
COPY public.account (id, login, email, password_hash, privilegy_level, description, profile_image) FROM stdin;
1	amongus	\N	1234567	1	\N	\N
2	trollface	\N	1234567	0	\N	\N
\.


--INSERT INTO public.account (id, login, email, password_hash, privilegy_level, description, profile_image) VALUES
--(1,amongus,null,'1234567',1,null,null),
--(2,trollface,null,'1234567',1,null,null);