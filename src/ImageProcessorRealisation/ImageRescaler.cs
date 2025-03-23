using CompetitiveBackend.Core.Objects;
using ImageMagick;
using Microsoft.Extensions.Logging;
using CompetitiveBackend.Services.ExtraTools;
using CompetitiveBackend.Services.Exceptions;
using System.Drawing;

namespace ImageProcessorRealisation
{
    class ImageRescaler : IImageProcessor
    {
        private ILogger _logger;
        private IImageConfig _config;
        public ImageRescaler(ILogger logger, IImageConfig config)
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
                using (var image = new MagickImage(data.Data))
                {
                    if(image.Width < _config.MinWidth || image.Height < _config.MaxWidth)
                    {
                        _logger.LogWarning("Provided image is too small, exiting");
                        throw new BadImageException("Provided image is too small");
                    }
                    if (image.HasAlpha)
                    {
                        image.BackgroundColor = MagickColor.FromRgb(0, 0, 0);
                        image.Alpha(AlphaOption.Remove);
                    }

                    uint newWidth = Math.Min(image.Width, _config.MaxWidth);
                    uint newHeight = Math.Min(image.Height, _config.MaxHeight);

                    image.Resize(newWidth, newHeight);
                    result = new LargeData(image.ToByteArray());
                    _logger.LogInformation("Image resized successfully");
                }
                return await Task.FromResult(result);
            }
            catch (MagickException ex)
            {
                _logger.LogWarning("The provided bytes was not a valid image: " + ex.Message);
                throw new BadImageException("The provided bytes are not a valid image: " + ex.Message, ex);
            }
        }
    }
}
