FROM nginx:1.22.0-alpine
COPY . /usr/share/nginx/html
EXPOSE 80
