#!/usr/bin/env bash

echo "Stashing changes..."

git stash

echo "Pulling latest master..."

git pull

echo "Deploying in PRODUCTION mode..."

docker-compose up --detach --remove-orphans --build

echo "Migrating DB..."

docker-compose -f ./docker-compose.migrate.yml build migrate
docker-compose -f ./docker-compose.migrate.yml run migrate

echo "Done."