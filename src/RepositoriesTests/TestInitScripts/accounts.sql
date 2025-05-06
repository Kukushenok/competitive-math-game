
COPY public.account (id, login, username, email, password_hash, privilegy_level, description, profile_image) FROM stdin;
1	amongus	amongus	\N	1234567	1	\N	\\x000102
2	trollface	trollface	\N	1234567	0	\N	\N
3	munny	munny	email@email.com	1234567	0	munny_description	\N
\.


--INSERT INTO public.account (id, login, email, password_hash, privilegy_level, description, profile_image) VALUES
--(1,amongus,null,'1234567',1,null,null),
--(2,trollface,null,'1234567',1,null,null);