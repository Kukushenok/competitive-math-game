using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessorRealisation
{
    public class Options
    {
        private IServiceCollection collection;
        public Options(IServiceCollection collection)
        {
            this.collection = collection;
        }

        public Options AddConstraints(uint minWidth, uint maxWidth, uint minHeight, uint maxHeight)
        {
            collection.AddSingleton<IImageConfig>(new DefaultImageConfig()
            {
                MaxHeight = maxHeight,
                MinHeight = minHeight,
                MaxWidth = maxWidth,
                MinWidth = minWidth
            });
            return this;
        }
        public Options UseDefaultConstraints()
        {
            AddConstraints(16, 256, 16, 256);
            return this;
        }
    }
}
