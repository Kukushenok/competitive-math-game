package main

import (
	"imageprocessor-grpc-mock/gen"
	"imageprocessor-grpc-mock/internal/server"
	"log"
	"net/http"

	"golang.org/x/net/http2"
	"golang.org/x/net/http2/h2c"
	"google.golang.org/grpc"
)

func main() {

	grpcServer := grpc.NewServer()
	gen.RegisterImageProcessorServer(grpcServer, server.New())
	// Регистрируем сервисы...
	// imageprocessor.RegisterImageProcessorServer(grpcServer, &server{})
	handler := h2c.NewHandler(
		http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
			grpcServer.ServeHTTP(w, r)
		}),
		&http2.Server{},
	)

	log.Println("Starting gRPC server on :8080 (h2c)")
	if err := http.ListenAndServe(":8080", handler); err != nil {
		log.Fatal(err)
	}
}
