FROM nginx:1.27-alpine

COPY ./nginx/conf.d /etc/nginx/conf.d
COPY ./nginx/nginx.conf /etc/nginx/nginx.conf
COPY ./www /var/www

WORKDIR /var/www