COPY public.competition (id, competition_name, description, start_time, end_time, level_data) FROM stdin;
1	The name	The description	2025-03-30 09:09:46.95493	2025-03-30 09:09:56.954935	\\x
2	Yearwide	Year	2025-03-30 09:09:46.95493	2026-03-30 09:09:56.954935	\\x010203
3	ez	description_ez	2025-01-01 00:00:00.00000	2025-02-01 00:00:00.00000	\\x
\.

COPY public.reward_description (id, reward_name, description, icon_image, ingame_data) FROM stdin;
1	Hello	There	\N	\N
2	Basic	Text	\\x000102	\\x030405
\.

COPY public.account (id, login, username, email, password_hash, privilegy_level, description, profile_image) FROM stdin;
1	amongus	amongus	\N	1234567	1	\N	\\x000102
2	trollface	trollface	\N	1234567	0	\N	\N
3	munny	munny	email@email.com	1234567	0	munny_description	\N
\.


COPY public.player_reward (id, reward_description_id, player_id, competition_id, creation_date) FROM stdin;
1	1	1	1	2025-04-01 18:43:26.531288
2	2	2	2	2025-04-01 18:43:26.531288
3	1	1	3	2025-04-01 18:43:26.531288
4	2	1	\N	2025-04-01 18:43:26.531288
\.

