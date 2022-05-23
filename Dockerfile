FROM nginx:1.20-alpine
COPY . /usr/share/nginx/html
EXPOSE 80
