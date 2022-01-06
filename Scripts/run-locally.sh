#!/usr/bin/bash

echo "Launching Api and Database..."

docker-compose -f ../docker-compose.local.yml up --build

echo "Done."