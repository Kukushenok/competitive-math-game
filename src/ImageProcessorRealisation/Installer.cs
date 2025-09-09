using CompetitiveBackend.Services.ExtraTools;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessorRealisation
{
    public static class Installer
    {
        public static IServiceCollection AddMajickImageRescaler(this IServiceCollection container, Action<Options>? setup = null)
        {
            container.AddScoped<IImageProcessor, ImageRescaler>();
            Options opt = new Options(container);
            if (setup != null) setup.Invoke(opt);
            if (!opt.ConfigurationSetUp) opt.UseDefaultConstraints();
            return container;
        }
    }
}
