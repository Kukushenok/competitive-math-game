using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Services.ExtraTools;
using Microsoft.Extensions.Configuration;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;

namespace CompetitiveBackend.Services.Objects
{
    public interface IImageEffector: ILargeFileProcessor
    {
    }
    public class ImageEffector : IImageEffector
    {
        private IImageProcessor processor;
        private uint MinSize;
        private uint MaxSize;
        public ImageEffector(IImageProcessor processor, IConfiguration conf)
        {
            this.processor = processor;
            var sec = conf.GetSection("Constraints:ImageConfig");
            MinSize = uint.TryParse(sec[nameof(MinSize)], out uint result) ? result: 32;
            MaxSize = uint.TryParse(sec[nameof(MaxSize)], out uint res2) ? res2 : 256;
        }

        public async Task<LargeData> Process(LargeData data)
        {
            data = await processor.Resize(data, MinSize, MaxSize);
            data = await processor.FitInBox(data);
            return data;
        }
    }
}
