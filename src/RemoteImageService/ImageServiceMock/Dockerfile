# build
FROM golang:1.24-alpine AS builder
WORKDIR /src

RUN apk add --no-cache git ca-certificates


COPY RemoteImageService/ImageServiceMock/ ./
RUN go mod download


COPY . .
RUN CGO_ENABLED=0 GOOS=linux go build -o /out/server ./cmd/server


# runtime
FROM alpine:3.19
RUN apk add --no-cache ca-certificates
COPY --from=builder /out/server /usr/local/bin/server


EXPOSE 8080
EXPOSE 8081
ENTRYPOINT ["/usr/local/bin/server"]