FROM nginx:latest
COPY ./nginx-linux-http.conf /etc/nginx/nginx.conf
RUN mkdir -p /var/www/cache

CMD ["nginx", "-g", "daemon off;"]
