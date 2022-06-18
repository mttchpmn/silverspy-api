#! /usr/bin/env bash

echo "Launching DB shell"

docker exec -it DATABASE psql -U postgres -d silverspy