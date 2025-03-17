create table if not exists account(
	id serial primary key,
	login varchar(32) unique not null, 
	email varchar(32),
	password_hash varchar,
	privilegy_level int,
	description varchar(128),
	profile_image bytea,
	check (login not like '% %')
);
