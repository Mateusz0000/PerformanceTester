FROM haproxy:1.8
COPY haproxy.cfg /usr/local/etc/haproxy/haproxy.cfg

RUN mkdir -p /run/haproxy

COPY localhost.pem /usr/local/etc/haproxy/localhost.pem
