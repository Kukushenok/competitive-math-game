COPY public.competition (id, competition_name, description, start_time, end_time) FROM stdin;
1	The name	The description	2025-03-30 09:09:46.95493	2025-03-30 09:09:56.954935
2	Yearwide	Year	2025-03-30 09:09:46.95493	2026-03-30 09:09:56.954935
3	ez	description_ez	2025-01-01 00:00:00.00000	2025-02-01 00:00:00.00000
\.

COPY public.competition_level (id, competition_id, version_key, platform, level_data) FROM stdin;
1	1	5	Android	\\x010203
2	1	6	Android	\\x040506
3	1	5	Windows	\\x070809
\.

--INSERT INTO public.account (id, login, email, password_hash, privilegy_level, description, profile_image) VALUES
--(1,amongus,null,'1234567',1,null,null),
--(2,trollface,null,'1234567',1,null,null);