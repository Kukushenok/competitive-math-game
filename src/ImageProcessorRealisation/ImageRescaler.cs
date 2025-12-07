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
        public ImageRescaler(ILogger<ImageRescaler> logger)
        {
            _logger = logger;
        }

        public Task<LargeData> FitInBox(LargeData data, uint colorRGBA = 255)
        {
            if (data.Data == null || data.Data.Length == 0) throw new BadImageException("Empty image");
            try
            {
                LargeData result;
                using (_logger.BeginScope("Image processing..."))
                using (var image = new MagickImage(data.Data))
                {
                    uint mx = Math.Max(image.Width, image.Height);
                    // Create an 80x80 blue background
                    using (var background = new MagickImage(MagickColor.FromRgba((byte)((colorRGBA >> 24) & 255), (byte)((colorRGBA >> 16) & 255), (byte)((colorRGBA >> 8) & 255), (byte)(colorRGBA & 255)), mx, mx))
                    {
                        // Composite the resized image onto the center of the background
                        background.Composite(image, Gravity.Center, CompositeOperator.Over);
                        background.Format = MagickFormat.Jpeg;
                        result = new LargeData(background.ToByteArray());
                        _logger.LogInformation("Image resized successfully");
                    }
                }
                return Task.FromResult(result);
            }
            catch (MagickException ex)
            {
                _logger.LogWarning("The provided bytes was not a valid image: " + ex.Message);
                throw new BadImageException("The provided bytes are not a valid image: " + ex.Message, ex);
            }
        }

        public async Task<LargeData> Resize(LargeData data, uint minSize, uint maxSize)
        {
            if (data.Data == null || data.Data.Length == 0) throw new BadImageException("Empty image");
            try
            {
                LargeData result;
                using (_logger.BeginScope("Image processing..."))
                using (var image = new MagickImage(data.Data))
                {
                    if(image.Width < minSize || image.Height < minSize)
                    {
                        _logger.LogWarning("Provided image is too small, exiting");
                        throw new BadImageException("Provided image is too small");
                    }
                    uint mx = Math.Min(Math.Max(image.Width, image.Height), maxSize);
                    image.Resize(new MagickGeometry($"{mx}x{mx}"));
                    result = new LargeData(image.ToByteArray());
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
