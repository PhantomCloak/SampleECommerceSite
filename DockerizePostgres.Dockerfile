FROM postgres:12.4

RUN apt-get update \
    && apt-get install wget -y \
    && apt-get install postgresql-12-postgis-3 -y \
    && apt-get install postgis -y

COPY ./postgres-init.sql /docker-entrypoint-initdb.d/
