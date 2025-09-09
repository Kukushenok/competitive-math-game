COPY public.competition (id, competition_name, description, start_time, end_time) FROM stdin;
1	The name	The description	2025-03-30 09:09:46.95493	2025-03-30 09:09:56.954935
2	Yearwide	Year	2025-03-30 09:09:46.95493	2026-03-30 09:09:56.954935
3	ez	description_ez	2025-01-01 00:00:00.00000	2025-02-01 00:00:00.00000
\.

COPY public.reward_description (id, reward_name, description, icon_image) FROM stdin;
1	Hello	There	\N
2	Basic	Text	\\x000102
\.

COPY public.account (id, login, username, email, password_hash, privilegy_level, description, profile_image) FROM stdin;
1	amongus	amongus	\N	1234567	1	\N	\\x000102
2	trollface	trollface	\N	1234567	0	\N	\N
3	munny	munny	email@email.com	1234567	0	munny_description	\N
\.

COPY public.competition_reward (id, reward_description_id, competition_id, condition_type, min_place, max_place, min_rank, max_rank) FROM stdin;
1	1	1	place	1	2	\N	\N
2	2	2	rank	\N	\N	0.5	1.0
3	1	2	rank	\N	\N	0.25	0.5
\.
