COPY public.competition (id, competition_name, description, start_time, end_time, level_data) FROM stdin;
1	The name	The description	2025-03-30 09:09:46.95493	2025-03-30 09:09:56.954935	\\x
2	Yearwide	Year	2025-03-30 09:09:46.95493	2026-03-30 09:09:56.954935	\\x010203
3	ez	description_ez	2025-01-01 00:00:00.00000	2025-02-01 00:00:00.00000	\\x
\.

--INSERT INTO public.account (id, login, email, password_hash, privilegy_level, description, profile_image) VALUES
--(1,amongus,null,'1234567',1,null,null),
--(2,trollface,null,'1234567',1,null,null);