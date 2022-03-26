FROM nginx:1.21-alpine
COPY . /usr/share/nginx/html
EXPOSE 80