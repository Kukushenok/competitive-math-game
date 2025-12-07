using Grpc.Core;
using ImageMagick;
using global::ImageProcessorService;

namespace ImageService.Services
{
    public class ImageProcessorServer : ImageProcessor.ImageProcessorBase
    {
        private readonly ILogger<ImageProcessorServer> _logger;

        public ImageProcessorServer(ILogger<ImageProcessorServer> logger)
        {
            _logger = logger;
        }

        public override async Task<ImageResponse> Resize(ResizeRequest request, ServerCallContext context)
        {
            try
            {
                if (request.Data == null || request.Data.Length == 0)
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Empty image"));

                using (_logger.BeginScope("Image processing..."))
                using (var image = new MagickImage(request.Data.ToByteArray()))
                {
                    if (image.Width < request.MinSize || image.Height < request.MinSize)
                    {
                        _logger.LogWarning("Provided image is too small, exiting");
                        throw new RpcException(new Status(StatusCode.InvalidArgument, "Provided image is too small"));
                    }

                    uint mx = Math.Min(Math.Max((uint)image.Width, (uint)image.Height), request.MaxSize);
                    image.Resize(new MagickGeometry($"{mx}x{mx}"));

                    var result = new ImageResponse
                    {
                        Data = Google.Protobuf.ByteString.CopyFrom(image.ToByteArray())
                    };

                    _logger.LogInformation("Image resized successfully");
                    return await Task.FromResult(result);
                }
            }
            catch (MagickException ex)
            {
                _logger.LogWarning("The provided bytes was not a valid image: " + ex.Message);
                throw new RpcException(new Status(StatusCode.InvalidArgument,
                    "The provided bytes are not a valid image: " + ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing image");
                throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
            }
        }

        public override async Task<ImageResponse> FitInBox(FitInBoxRequest request, ServerCallContext context)
        {
            try
            {
                if (request.Data == null || request.Data.Length == 0)
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Empty image"));

                uint colorRGBA = request.ColorRGBA == 0 ? 255 : request.ColorRGBA;

                using (_logger.BeginScope("Image processing..."))
                using (var image = new MagickImage(request.Data.ToByteArray()))
                {
                    uint mx = Math.Max((uint)image.Width, (uint)image.Height);

                    // Создаем квадратный фон
                    using (var background = new MagickImage(MagickColor.FromRgba(
                            (byte)((colorRGBA >> 24) & 255),
                            (byte)((colorRGBA >> 16) & 255),
                            (byte)((colorRGBA >> 8) & 255),
                            (byte)(colorRGBA & 255)),
                        mx, mx))
                    {
                        // Накладываем изображение по центру
                        background.Composite(image, Gravity.Center, CompositeOperator.Over);
                        background.Format = MagickFormat.Jpeg;

                        var result = new ImageResponse
                        {
                            Data = Google.Protobuf.ByteString.CopyFrom(background.ToByteArray())
                        };

                        _logger.LogInformation("Image fitted in box successfully");
                        return await Task.FromResult(result);
                    }
                }
            }
            catch (MagickException ex)
            {
                _logger.LogWarning("The provided bytes was not a valid image: " + ex.Message);
                throw new RpcException(new Status(StatusCode.InvalidArgument,
                    "The provided bytes are not a valid image: " + ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing image");
                throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
            }
        }
    }
}

