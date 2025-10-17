cd ../../../

docker compose  -f ./docker-compose.yml -f ./docker-compose.k6.yml --profile scenario1 --ansi never build competitivebackend

docker compose  -f ./docker-compose.yml -f ./docker-compose.k6.yml --profile scenario1 --ansi never up -d --no-build