#!/usr/bin/bash

echo "Migrating Database..."

docker-compose -f ../Database.Migrator/docker-compose.migrate.yml build migrate &&
docker-compose -f ../Database.Migrator/docker-compose.migrate.yml run migrate

echo "Done."