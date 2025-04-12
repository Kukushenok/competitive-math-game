create table if not exists account(
	id int generated always as identity primary key,
	login varchar(32) unique not null, 
	email varchar(32),
	password_hash varchar,
	privilegy_level int,
	description varchar(128),
	profile_image bytea,
	check (login not like '% %')
);

create table if not exists reward_description(
	id int generated always as identity primary key,
	reward_name varchar(64) not null,
	description varchar(128),
	icon_image bytea,
	ingame_data bytea
);

create table if not exists competition(
	id int generated always as identity primary key,
	competition_name varchar(64) not null,
	description varchar(128),
	start_time timestamp not null,
	end_time timestamp not null,
	level_data bytea,
	has_ended bool not null default(false),
	check(end_time > start_time)
);

create table if not exists player_participation(
	competition_id int references competition(id) not null,
	account_id int references account(id) not null,
	score int not null
);

alter table player_participation add constraint comp_uniqueness unique (competition_id, account_id); 

create table if not exists competition_reward(
	id int generated always as identity primary key,
	reward_description_id int references reward_description(id) not null,
	competition_id int references competition(id) not null,
	condition jsonb
);

create table if not exists player_reward(
	id int generated always as identity primary key,
	reward_description_id int references reward_description(id) not null,
	player_id int references account(id) not null,
	competition_id int references competition(id),
	creation_date timestamp default(now())
);



create or replace function grant_place_rewards(sortedtable refcursor, min_val int, max_val int)
RETURNS SETOF player_participation  -- Use SETOF to return multiple rows
LANGUAGE plpgsql
AS $$
DECLARE
    row_data player_participation;
BEGIN
    -- Move to the starting position
    MOVE ABSOLUTE (min_val - 1) IN sortedtable;
    -- Fetch and return rows in a loop
    FOR i IN 1..(max_val - min_val + 1) LOOP
        FETCH NEXT FROM sortedtable INTO row_data;
        EXIT WHEN NOT FOUND;
        RETURN NEXT row_data;
    END LOOP;
    RETURN;
END;
$$;

create or replace function grant_rank_rewards(sortedtable refcursor, min_rank float, max_rank float)
RETURNS SETOF player_participation  
LANGUAGE plpgsql
AS $$
DECLARE
    row_count int := 0;
	min_val int := 0;
	max_val int := 0;
BEGIN
    -- Move to the end
    MOVE LAST IN sortedtable;
    -- Get the current position (which is the count)
    GET DIAGNOSTICS row_count = ROW_COUNT;
	MOVE ABSOLUTE 0 IN sortedtable;
	min_val := floor((1.0 - max_rank) * row_count);
	max_val := floor((1.0 - min_rank) * row_count);
	RETURN QUERY (select * from grant_place_rewards(sortedtable, min_val, max_val));
END;
$$;

-- deepseek my beloved goat
CREATE OR REPLACE PROCEDURE grant_rewards(comp_id int)
LANGUAGE plpgsql 
AS $$
DECLARE
    creward competition_reward%rowtype;
    comp_valid_id int := null;
    is_granted bool := false;
    reward_type text;
    place_cursor REFCURSOR := 'place_cursor';
    reward_cursor REFCURSOR := 'reward_cursor';
    player_rec player_participation%rowtype;
    min_condition float;
    max_condition float;
    min_place int;
    max_place int;
BEGIN
    -- Check if rewards already granted
    SELECT c.id, c.has_ended 
    INTO comp_valid_id, is_granted
    FROM competition c
    WHERE c.id = comp_id;
    IF comp_valid_id is null THEN
    	RAISE EXCEPTION 'There is no such competition' USING HINT = 'No competition with ID';
    ELSIF is_granted THEN
        RAISE EXCEPTION 'Rewards have already been granted' USING HINT = 'Rewards already granted.';
    ELSE
        -- Open the base cursor for place-based ordering
        OPEN place_cursor FOR 
            SELECT * FROM player_participation p 
            WHERE p.competition_id = comp_id
            ORDER BY p.score DESC;
        
        FOR creward IN 
            SELECT * FROM competition_reward c WHERE c.competition_id = comp_id
        LOOP
            reward_type := creward.condition->>'Type';
            BEGIN
	            CASE 
	                WHEN reward_type = 'rank' THEN
						IF NOT (creward.condition ?& array['minRank', 'maxRank']) THEN
							RAISE WARNING 'Processing rank-based reward %: Missing fields MinRank or MaxRank, skipping', creward.id;
							CONTINUE;
						END IF;
	                    min_condition := (creward.condition->>'minRank')::float;
	                    max_condition := (creward.condition->>'maxRank')::float;
	                    
	                    -- Open cursor for rank-based rewards
	                    OPEN reward_cursor FOR 
	                        SELECT * FROM grant_rank_rewards(place_cursor, min_condition, max_condition);
	                    
	                WHEN reward_type = 'place' THEN
						IF NOT (creward.condition ?& array['minPlace', 'maxPlace']) THEN
							RAISE WARNING 'Processing rank-based reward %: Missing fields MinPlace or MaxPlace, skipping', creward.id;
							CONTINUE;
						END IF;
	                    min_place := (creward.condition->>'minPlace')::int;
	                    max_place := (creward.condition->>'maxPlace')::int;
	                    
	                    -- Open cursor for place-based rewards
	                    OPEN reward_cursor FOR 
	                        SELECT * FROM grant_place_rewards(place_cursor, min_place, max_place);
	                    
	                ELSE
	                    RAISE WARNING 'Processing %: Unrecognized reward type %; skipping', creward.id, reward_type;
	                    CONTINUE;  -- Skip to next reward
	            END CASE;
		    EXCEPTION WHEN OTHERS THEN
		        RAISE WARNING 'Processing %: Type cast error occured, skipping', creward.id;
		        CONTINUE;
            END;
            -- Process the reward cursor (common for both types)
            LOOP
                FETCH reward_cursor INTO player_rec;
                EXIT WHEN NOT FOUND;
                
                INSERT INTO player_reward(reward_description_id, player_id, competition_id)
                VALUES (creward.id, player_rec.account_id, comp_id);
            END LOOP;
            
            -- Clean up and reset for next iteration
            CLOSE reward_cursor;
            MOVE ABSOLUTE 0 IN place_cursor;
        END LOOP;
        
        -- Final clean up
        CLOSE place_cursor;
        UPDATE competition c SET has_ended = true WHERE c.id = comp_id;
    END IF;
END;
$$;