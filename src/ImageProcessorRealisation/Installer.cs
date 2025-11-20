using CompetitiveBackend.Services.ExtraTools;
using Microsoft.Extensions.DependencyInjection;

namespace ImageProcessorRealisation
{
    public static class Installer
    {
        public static IServiceCollection AddMajickImageRescaler(this IServiceCollection container, Action<Options>? setup = null)
        {
            container.AddScoped<IImageProcessor, ImageRescaler>();
            var opt = new Options(container);
            setup?.Invoke(opt);

            if (!opt.ConfigurationSetUp)
            {
                opt.UseDefaultConstraints();
            }

            return container;
        }
    }
}
