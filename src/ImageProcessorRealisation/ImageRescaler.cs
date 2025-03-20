using CompetitiveBackend.Core.Objects;
using ImageMagick;
using Microsoft.Extensions.Logging;
using CompetitiveBackend.Services.ExtraTools;
using CompetitiveBackend.Services.Exceptions;

namespace ImageProcessorRealisation
{
    class ImageRescaler : IImageProcessor
    {
        private ILogger<ImageRescaler> _logger;
        private IImageConfig _config;
        public ImageRescaler(ILogger<ImageRescaler> logger, IImageConfig config)
        {
            _logger = logger;
            _config = config;
        }
        public async Task<LargeData> Process(LargeData data)
        {
            try
            {
                LargeData result;
                using (_logger.BeginScope("Image processing..."))
                // Step 1: Validate that the bytes represent an image
                using (var image = new MagickImage(data.Data))
                {
                    if(image.Width < _config.MinWidth || image.Height < _config.MaxWidth)
                    {
                        throw new BadImageException("Provided image is too small");
                    }
                    // Step 2: Rescale the image to an appropriate size
                    uint newWidth = Math.Min(image.Width, _config.MaxWidth);  // Desired width
                    uint newHeight = Math.Min(image.Height, _config.MaxHeight); // Desired height

                    image.Resize(newWidth, newHeight);

                    // Step 3: Save the rescaled image as a byte array
                    result = new LargeData(image.ToByteArray());
                    _logger.LogInformation("Image resized successfully");
                }
                return await Task.FromResult(result);
            }
            catch (MagickException ex)
            {
                _logger.LogWarning("The provided bytes are not a valid image: " + ex.Message);
                throw new BadImageException("The provided bytes are not a valid image: " + ex.Message, ex);
            }
        }
    }
}
