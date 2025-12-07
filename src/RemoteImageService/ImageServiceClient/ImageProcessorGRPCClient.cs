using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Services.Exceptions;
using CompetitiveBackend.Services.ExtraTools;
using Grpc.Core;
using Grpc.Net.Client;
using ImageProcessorService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
namespace ImageServiceClient
{
    internal class ImageProcessorGrpcClient : IImageProcessor, IDisposable
    {
        private readonly GrpcChannel _channel;
        private readonly ImageProcessor.ImageProcessorClient _client;
        private readonly ILogger<ImageProcessorGrpcClient> _logger;

        public ImageProcessorGrpcClient(IConfiguration conf, ILogger<ImageProcessorGrpcClient> logger)
        {
            _logger = logger;

            // Настройка канала
            var channelOptions = new GrpcChannelOptions
            {
                HttpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                }
            };

            _channel = GrpcChannel.ForAddress(conf.GetConnectionString("ImageService") ?? "grpc://image-processor:8081", channelOptions);
            _client = new ImageProcessor.ImageProcessorClient(_channel);
        }

        public async Task<LargeData> Resize(LargeData data, uint minSize, uint maxSize)
        {
            try
            {
                var request = new ResizeRequest
                {
                    Data = Google.Protobuf.ByteString.CopyFrom(data.Data),
                    MinSize = minSize,
                    MaxSize = maxSize
                };

                var response = await _client.ResizeAsync(request);
                return new LargeData(response.Data.ToByteArray());
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, "Error calling Resize via gRPC");
                throw new BadImageException(ex.Status.Detail, ex);
            }
        }

        public async Task<LargeData> FitInBox(LargeData data, uint colorRGBA = 0x000000FF)
        {
            try
            {
                var request = new FitInBoxRequest
                {
                    Data = Google.Protobuf.ByteString.CopyFrom(data.Data),
                    ColorRGBA = colorRGBA
                };

                var response = await _client.FitInBoxAsync(request);
                return new LargeData(response.Data.ToByteArray());
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, "Error calling FitInBox via gRPC");
                throw new BadImageException(ex.Status.Detail, ex);
            }
        }

        public void Dispose()
        {
            _channel?.Dispose();
        }
    }
}