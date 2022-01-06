#!/usr/bin/bash

echo "Launching Database service..."

docker-compose -f ../docker-compose.local.yml run --service-ports db

echo "Done."