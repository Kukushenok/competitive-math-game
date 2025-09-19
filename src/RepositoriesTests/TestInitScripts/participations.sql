
-- Data for Name: account; Type: TABLE DATA; Schema: public; Owner: tester
--
COPY public.account (id, login, username, email, password_hash, privilegy_level, description, profile_image) FROM stdin;
1	Klavdiya_Ilin41_0	Klavdiya_Ilin41_0	Ignatii_Terentev@hotmail.com	HASH	0	\N	\N
2	Aleksandr35_1	Aleksandr35_1	Irina.Shestakov@yahoo.com	HASH	0	\N	\N
3	Arina.Nosov63_9	Arina.Nosov63_9	Oksana.Kabanova20@yahoo.com	HASH	0	\N	\N
4	Grigorii.Ryabova85_11	Grigorii.Ryabova85_11	Aleksandra82@ya.ru	HASH	0	\N	\N
5	Georgii.Soloveva3_15	Georgii.Soloveva3_15	Rimma61@mail.ru	HASH	0	\N	\N
6	Funnnn	Klavdiya_Ilin41_0	Ignatii_Terentev@hotmail.com	HASH	0	\N	\N
\.


--
-- Data for Name: competition; Type: TABLE DATA; Schema: public; Owner: tester
--

COPY public.competition (id, competition_name, description, start_time, end_time, has_ended) FROM stdin;
1	Свободный Стальной Куртка	Количественный всего требует нац	2025-04-30 14:42:54.302608	2025-05-12 07:20:03.884016	f
\.


--
-- Data for Name: competition_reward; Type: TABLE DATA; Schema: public; Owner: tester
--

COPY public.competition_reward (id, reward_description_id, competition_id, condition_type, min_place, max_place, min_rank, max_rank) FROM stdin;
\.


--
-- Data for Name: player_participation; Type: TABLE DATA; Schema: public; Owner: tester
--

COPY public.player_participation (competition_id, account_id, score, last_update_time) FROM stdin;
1	1	152	2025-05-03 10:49:02.603038
1	2	215	2025-05-01 09:12:23.484699
1	3	763	2025-04-24 04:00:26.088876
1	4	1	2025-04-17 10:57:33.04738
1	5	840	2025-04-25 16:54:16.79832
\.


--
-- Data for Name: player_reward; Type: TABLE DATA; Schema: public; Owner: tester
--

COPY public.player_reward (id, reward_description_id, player_id, competition_id, creation_date) FROM stdin;
\.


--
-- Data for Name: reward_description; Type: TABLE DATA; Schema: public; Owner: tester
--

COPY public.reward_description (id, reward_name, description, icon_image) FROM stdin;
3	Грубый Кожанный Клатч	Высокотехнологичная активности р	\N
1	Интеллектуальный Гранитный Стул	Нашей оценить участия правительс	\N
5	Фантастический Бетонный Свитер	Нашей деятельности значение стор	\N
2	Свободный Кожанный Майка	Играет управление разработке с н	\N
4	Невероятный Хлопковый Стул	Позиции уточнения прежде же вызы	\N
\.
