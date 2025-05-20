using CompetitiveBackend.Core.Objects;
using ImageMagick;
using Microsoft.Extensions.Logging;
using CompetitiveBackend.Services.ExtraTools;
using CompetitiveBackend.Services.Exceptions;
using System.Drawing;
using ImageMagick.Drawing;

namespace ImageProcessorRealisation
{
    internal class ImageRescaler : IImageProcessor
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
            if (data.Data == null || data.Data.Length == 0) throw new BadImageException("Empty image");
            try
            {
                LargeData result;
                using (_logger.BeginScope("Image processing..."))
                using (var image = new MagickImage(data.Data))
                {
                    if(image.Width < _config.MinWidth || image.Height < _config.MinHeight)
                    {
                        _logger.LogWarning("Provided image is too small, exiting");
                        throw new BadImageException("Provided image is too small");
                    }
                    if (image.HasAlpha)
                    {
                        image.BackgroundColor = MagickColor.FromRgb(0, 0, 0);
                        image.Alpha(AlphaOption.Remove);
                    }
                    float ratio = (float)image.Width / image.Height;
                    uint newWidth, newHeight;
                    if (image.Height > image.Width)
                    {
                        newHeight = Math.Min(image.Height, _config.MaxHeight);
                        newWidth = Math.Min((uint)(newHeight * ratio), _config.MaxWidth);
                    }
                    else
                    {
                        newWidth = Math.Min(image.Width, _config.MaxWidth);
                        newHeight = Math.Min((uint)(newWidth / ratio), _config.MaxHeight);
                    }
                    uint mx = Math.Max(newWidth, newHeight);
                    image.Resize(newWidth, newHeight);
                    image.BorderColor = image.BackgroundColor;
                    image.Border((mx - newWidth) / 2, (mx - newHeight) / 2);

                    image.Format = MagickFormat.Jpeg;
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
