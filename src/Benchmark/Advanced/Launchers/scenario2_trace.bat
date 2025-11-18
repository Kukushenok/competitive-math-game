cd ../../../

docker compose -f ./docker-compose.yml -f ./docker-compose.tracing.yml -f ./docker-compose.k6.yml --profile scenario2 --ansi never build competitivebackend

docker compose -f ./docker-compose.yml -f ./docker-compose.tracing.yml -f ./docker-compose.k6.yml --profile scenario2 --ansi never up -d --no-build