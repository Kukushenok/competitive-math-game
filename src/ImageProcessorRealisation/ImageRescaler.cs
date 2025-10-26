using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Services.Exceptions;
using CompetitiveBackend.Services.ExtraTools;
using ImageMagick;
using Microsoft.Extensions.Logging;

namespace ImageProcessorRealisation
{
    internal sealed class ImageRescaler : IImageProcessor
    {
        private readonly ILogger<ImageRescaler> logger;
        private readonly IImageConfig config;
        public ImageRescaler(ILogger<ImageRescaler> logger, IImageConfig config)
        {
            this.logger = logger;
            this.config = config;
        }

        private LargeData ProcessImage(LargeData data)
        {
            LargeData result;
            using (logger.BeginScope("Image processing..."))
            using (var image = new MagickImage(data.Data))
            {
                if (image.Width < config.MinSize || image.Height < config.MinSize)
                {
                    logger.LogWarning("Provided image is too small, exiting");
                    throw new BadImageException("Provided image is too small");
                }

                uint mx = Math.Min(Math.Max(image.Width, image.Height), config.MaxSize);
                image.Resize(new MagickGeometry($"{mx}x{mx}"));

                // Create an 80x80 blue background
                using var background = new MagickImage(MagickColors.Black, mx, mx);

                // Composite the resized image onto the center of the background
                background.Composite(image, Gravity.Center, CompositeOperator.Over);
                background.Format = MagickFormat.Jpeg;
                result = new LargeData(background.ToByteArray());
                logger.LogInformation("Image resized successfully");
            }

            return result;
        }

        public async Task<LargeData> Process(LargeData data)
        {
            if (data.Data == null || data.Data.Length == 0)
            {
                throw new BadImageException("Empty image");
            }

            try
            {
                return await Task.FromResult(ProcessImage(data));
            }
            catch (MagickException ex)
            {
                logger.LogWarning("The provided bytes was not a valid image: " + ex.Message);
                throw new BadImageException("The provided bytes are not a valid image: " + ex.Message, ex);
            }
        }
    }
}
