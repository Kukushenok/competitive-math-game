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
                    if(image.Width < _config.MinSize || image.Height < _config.MinSize)
                    {
                        _logger.LogWarning("Provided image is too small, exiting");
                        throw new BadImageException("Provided image is too small");
                    }
                    uint mx = Math.Min(Math.Max(image.Width, image.Height), _config.MaxSize);
                    image.Resize(new MagickGeometry($"{mx}x{mx}"));

                    // Create an 80x80 blue background
                    using (var background = new MagickImage(MagickColors.Black, mx, mx))
                    {
                        // Composite the resized image onto the center of the background
                        background.Composite(image, Gravity.Center, CompositeOperator.Over);
                        background.Format = MagickFormat.Jpeg;
                        result = new LargeData(background.ToByteArray());
                        _logger.LogInformation("Image resized successfully");
                    }
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
