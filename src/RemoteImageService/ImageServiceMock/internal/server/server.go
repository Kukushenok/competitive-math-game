package server

import (
	"bytes"
	"context"
	"fmt"
	"image"
	_ "image/gif"
	_ "image/jpeg"
	_ "image/png"

	"imageprocessor-grpc-mock/gen"

	"google.golang.org/grpc/codes"
	"google.golang.org/grpc/status"
)

type svc struct {
	gen.UnimplementedImageProcessorServer
}

func New() *svc { return &svc{} }

func validateImage(data []byte) error {
	if len(data) == 0 {
		return fmt.Errorf("empty data")
	}
	r := bytes.NewReader(data)
	_, _, err := image.Decode(r)
	if err != nil {
		return err
	}
	return nil
}

func (s *svc) Resize(ctx context.Context, req *gen.ResizeRequest) (*gen.ImageResponse, error) {
	if err := validateImage(req.GetData()); err != nil {
		return nil, status.Errorf(codes.InvalidArgument, "invalid image: %v", err)
	}
	return &gen.ImageResponse{Data: req.GetData()}, nil
}

func (s *svc) FitInBox(ctx context.Context, req *gen.FitInBoxRequest) (*gen.ImageResponse, error) {
	if err := validateImage(req.GetData()); err != nil {
		return nil, status.Errorf(codes.InvalidArgument, "invalid image: %v", err)
	}
	return &gen.ImageResponse{Data: req.GetData()}, nil
}
