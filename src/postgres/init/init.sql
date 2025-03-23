create table if not exists account(
	id serial primary key,
	login varchar(32) unique not null, 
	email varchar(32),
	password_hash varchar,
	privilegy_level int,
	--description varchar(128),
	--profile_image bytea,
	check (login not like '% %')
);

create table if not exists profile(
	id int unique references account(id) primary key,
	description varchar(128),
	profile_image bytea
);

create table if not exists reward_description(
	id serial primary key,
	reward_name varchar(64) not null,
	description varchar(128),
	icon_image bytea,
	ingame_data bytea
);

create table if not exists competition(
	id serial primary key,
	competition_name varchar(64) not null,
	description varchar(128),
	start_time timestamp not null,
	end_time timestamp not null,
	check(end_time > start_time)
)

create table if not exists player_participation(
	competition_id int references competition(id) not null,
	account_id int references account(id) not null,
	score int not null
);

create table if not exists competition_reward(
	id serial primary key,
	reward_description_id int references reward_description(id) not null,
)