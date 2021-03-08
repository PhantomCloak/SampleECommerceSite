!#/bin/bash

sudo -u postgres psql -c 'create database efurni;'
sudo -u postgres psql -c 'create database location;'

dotnet build

psql -U postgres -d efurni < production-db.sql 
psql -U postgres -d efurni < location-db.sql

pg_restore -c -U postgres -d efurni -v production-example-dump.tar
pg_restore -c -U postgres -d location -v location-example-dump.tar


