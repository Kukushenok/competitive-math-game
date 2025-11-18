cd ../../../

docker compose  -f ./docker-compose.yml -f ./docker-compose.k6.yml --profile scenario3 --ansi never build competitivebackend

docker compose  -f ./docker-compose.yml -f ./docker-compose.k6.yml --profile scenario3 --ansi never up -d --no-build