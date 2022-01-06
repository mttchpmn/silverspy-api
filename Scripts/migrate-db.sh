#!/usr/bin/bash

echo "Migrating Database..."

docker-compose -f ../docker-compose.migrate.yml build migrate &&
docker-compose -f ../docker-compose.migrate.yml run migrate

echo "Done."