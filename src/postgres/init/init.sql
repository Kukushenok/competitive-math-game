create table if not exists account(
	id int generated always as identity primary key,
	login varchar(32) unique not null, 
	username varchar(32) not null,
	email varchar(32),
	password_hash varchar,
	privilegy_level int,
	description varchar(128),
	profile_image bytea
);

alter table account 
add constraint proper_email check (email ~* '^[A-Za-z0-9._+%-]+@[A-Za-z0-9.-]+[.][A-Za-z]+$'),
add constraint proper_login check (login not like '% %');

create table if not exists reward_description(
	id int generated always as identity primary key,
	reward_name varchar(64) not null,
	description varchar(128),
	icon_image bytea
);

create table if not exists competition(
	id int generated always as identity primary key,
	competition_name varchar(64) not null,
	description varchar(128),
	start_time timestamp not null,
	end_time timestamp not null,
	has_ended bool not null default(false)
);

alter table competition add constraint start_end_coherency check (end_time > start_time); 

create table if not exists competition_level(
	id int generated always as identity primary key,
	competition_id int references competition(id) not null,
	version_key int not null,
	platform varchar(32) not null,
	level_data bytea not null
);

create table if not exists player_participation(
	competition_id int references competition(id) not null,
	account_id int references account(id) not null,
	score int not null,
	last_update_time timestamp not null default(now()) 
);

alter table player_participation add constraint comp_uniqueness unique (competition_id, account_id); 

create type condition_type_enum as enum ('place', 'rank');

create table competition_reward (
    id int generated always as identity primary key,
    reward_description_id int references reward_description(id) not null,
    competition_id int references competition(id) not null,
    condition_type condition_type_enum not null,
    min_place int,
    max_place int,
    min_rank decimal(4,3),
    max_rank decimal(4,3)
);

alter table competition_reward
add constraint chk_place_min check (
    condition_type <> 'place' or (min_place is not null and min_place >= 1)
),
add constraint chk_place_max check (
    condition_type <> 'place' or (max_place is not null and max_place >= min_place)
),
add constraint chk_rank_min check (
    condition_type <> 'rank' or (min_rank is not null and min_rank >= 0)
),
add constraint chk_rank_max check (
    condition_type <> 'rank' or (max_rank is not null and max_rank <= 1)
),
add constraint chk_rank_range check (
    condition_type <> 'rank' or (max_rank >= min_rank)
);

create table if not exists player_reward(
	id int generated always as identity primary key,
	reward_description_id int references reward_description(id) not null,
	player_id int references account(id) not null,
	competition_id int references competition(id),
	creation_date timestamp default(now())
);

create or replace function grant_place_rewards(sortedtable refcursor, min_val int, max_val int)
returns setof player_participation  -- use setof to return multiple rows
language plpgsql
as $$
declare
    row_data player_participation;
begin
    move absolute (min_val - 1) in sortedtable;
    for i in 1..(max_val - min_val + 1) loop
        fetch next from sortedtable into row_data;
        exit when not found;
        return next row_data;
    end loop;
    return;
end;
$$;

create or replace function grant_rank_rewards(sortedtable refcursor, min_rank float, max_rank float)
returns setof player_participation  
language plpgsql
as $$
declare
    row_cnt int := 0;
	min_val int := 0;
	max_val int := 0;
begin
    move absolute 0 in sortedtable;
    move forward all from sortedtable;
    get diagnostics row_cnt = row_count;
	move absolute 0 in sortedtable;

	min_val := floor((1.0 - max_rank) * row_cnt) + 1;
	max_val := ceil((1.0 - min_rank) * row_cnt) + 1;

	return query (select * from grant_place_rewards(sortedtable, min_val, max_val));
end;
$$;

create or replace procedure grant_rewards(comp_id int)
language plpgsql 
as $$
declare
    creward competition_reward%rowtype;
    comp_valid_id int := null;
    is_granted bool := false;
    reward_type condition_type_enum;
    place_cursor refcursor := 'place_cursor';
    reward_cursor refcursor := 'reward_cursor';
    player_rec player_participation%rowtype;
begin
    select c.id, c.has_ended 
    into comp_valid_id, is_granted
    from competition c
    where c.id = comp_id;
    if comp_valid_id is null then
    	raise exception 'There is no such competition' using hint = 'No competition with ID';
    elsif is_granted then
        raise exception 'Rewards have already been granted' using hint = 'Rewards already granted.';
    else
        update competition c set has_ended = true where c.id = comp_id;
        open place_cursor scroll for 
            select * from player_participation p 
            where p.competition_id = comp_id
            order by p.score desc, p.last_update_time asc;
        
        for creward in 
            select * from competition_reward c where c.competition_id = comp_id
        loop
            reward_type := creward.condition_type;
            case 
                when reward_type = 'rank' then
                    open reward_cursor for 
                        select * from grant_rank_rewards(place_cursor, creward.min_rank, creward.max_rank);
                    
                when reward_type = 'place' then
                    open reward_cursor for 
                        select * from grant_place_rewards(place_cursor, creward.min_place, creward.max_place);
                    
                else
                    RAISE WARNING 'Processing %: Unrecognized reward type %; skipping', creward.id, reward_type;
                    continue;  -- skip to next reward
            end case;
            loop
                fetch reward_cursor into player_rec;
                exit when not found;
                
                insert into player_reward(reward_description_id, player_id, competition_id)
                values (creward.reward_description_id, player_rec.account_id, comp_id);
            end loop;
            close reward_cursor;
            move absolute 0 in place_cursor;
        end loop;
        close place_cursor;
    end if;
end;
$$;

create or replace function public.check_password_hash(
    p_login varchar,
    p_input_hash varchar
)
returns boolean as $$
declare
    v_stored_hash varchar;
    v_result boolean;
begin
    select password_hash from account where login = p_login into v_stored_hash;
    select v_stored_hash = p_input_hash into v_result;
    
    return v_result;
end;
$$ language plpgsql security definer;

-- guest

revoke select, update, delete on account from public;
create role guest with login password 'guest_password';
grant select on competition, competition_reward, player_participation, reward_description, competition_level to guest;
grant select (id, login, username, email, privilegy_level, description, profile_image) on account to guest;
grant insert (login, username, email, password_hash, privilegy_level) on account to guest;
grant execute on function check_password_hash(varchar, varchar) to guest;
alter role guest with inherit;

-- player

create role player with login password 'player_password';
grant guest to player;
grant update (username, description, profile_image) on account to player;
grant select, insert, update on player_participation to player;
grant select on player_reward to player;
alter role player inherit;

-- admin

create role admin with login password 'admin_password';
grant guest to admin;
grant select, delete on player_participation to admin;
grant select, insert, update, delete on player_reward, competition_level to admin;
grant insert, update on competition, competition_reward, reward_description to admin;
alter role admin inherit;

-- reward_granter

create role reward_granter with login password 'reward_granter';
grant execute on procedure grant_rewards(integer) to reward_granter;
grant select, update on table competition to reward_granter;
grant select on table player_participation to reward_granter;
grant select on table competition_reward to reward_granter;
grant insert on table player_reward to reward_granter;


