FROM nginx:1.22-alpine
COPY . /usr/share/nginx/html
EXPOSE 80
