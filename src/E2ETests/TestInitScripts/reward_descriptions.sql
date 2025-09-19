COPY public.reward_description (id, reward_name, description, icon_image) FROM stdin;
1	Hello	There	\N
2	Basic	Text	\\x000102
\.

--INSERT INTO public.account (id, login, email, password_hash, privilegy_level, description, profile_image) VALUES
--(1,amongus,null,'1234567',1,null,null),
--(2,trollface,null,'1234567',1,null,null);