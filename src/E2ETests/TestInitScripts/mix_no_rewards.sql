COPY public.competition (id, competition_name, description, start_time, end_time) FROM stdin;
1	The name	The description	2025-03-30 09:09:46.95493	2027-03-30 09:09:56.954935
2	Yearwide	Year	2025-03-30 09:09:46.95493	2026-03-30 09:09:56.954935
3	ez	description_ez	2025-01-01 00:00:00.00000	2025-02-01 00:00:00.00000
\.

COPY public.reward_description (id, reward_name, description, icon_image) FROM stdin;
1	Hello	There	\N
2	Basic	Text	\\x000102
\.
