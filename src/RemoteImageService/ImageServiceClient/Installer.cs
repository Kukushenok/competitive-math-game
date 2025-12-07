using CompetitiveBackend.Services.ExtraTools;
using ImageServiceClient;
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
        public static IServiceCollection AddImageRescalerClient(this IServiceCollection container)
        {
            container.AddScoped<IImageProcessor, ImageProcessorGrpcClient>();
            return container;
        }
    }
}
