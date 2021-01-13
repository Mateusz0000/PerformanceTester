#!/usr/bin/env bash

#echo on
set -x

docker run \
    -d \
    --log-opt max-size=10m \
    --log-opt max-file=3 \
    --name mssql-server \
    -p 1433:1433 \
    --restart always \
    -e 'ACCEPT_EULA=Y' \
    -e 'MSSQL_PID=Enterprise' \
    -e 'MSSQL_SA_PASSWORD=Benchmarkdbp@55' \
    mssql
