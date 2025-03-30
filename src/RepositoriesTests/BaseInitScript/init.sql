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
	condition_name varchar(32),
	condition_description varchar(32)
);

create table if not exists player_reward(
	id int generated always as identity primary key,
	reward_description_id int references reward_description(id) not null,
	player_id int references account(id) not null,
	competition_id int references competition(id),
	creation_date timestamp default(now())
);