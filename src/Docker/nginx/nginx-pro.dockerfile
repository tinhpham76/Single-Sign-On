FROM nginx:latest

COPY ./nginx-pro.conf /etc/nginx/nginx.conf

RUN mkdir -p /var/www/cache

RUN openssl req -x509 -subj /CN=localhost -days 365 -set_serial 2 -newkey rsa:4096 -keyout /etc/ssl/private/cert.key -nodes -out /etc/ssl/certs/cert.pem

CMD ["nginx", "-g", "daemon off;"]