FROM nginx:latest

COPY nginx.conf /etc/nginx/nginx.conf
COPY localhost.crt /etc/nginx/localhost.crt
COPY localhost.rsa /etc/nginx/localhost.rsa

RUN apt-get update && apt-get install -y iputils-ping
RUN apt-get install -y net-tools

ADD run.sh /
RUN chmod +x /run.sh

ENTRYPOINT ["/run.sh"]
