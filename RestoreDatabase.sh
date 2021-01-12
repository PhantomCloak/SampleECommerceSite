DB_IP=""

pg_restore -h $DB_IP -p 5000 -c -U service -d efurni -v "production-example-dump.tar" -W
pg_restore -h $DB_IP -p 5000 -c -U service -d locations -v "location-example-dump.tar" -W
