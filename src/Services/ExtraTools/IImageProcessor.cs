using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Services.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.Services.ExtraTools
{
    /// <summary>
    /// Логическое разделение: оно обрабатывает только изображения.
    /// </summary>
    public interface IImageProcessor
    {
        public Task<LargeData> Resize(LargeData data, uint minSize, uint maxSize);
        public Task<LargeData> FitInBox(LargeData data, uint colorRGBA = 0x000000FF);
    }
}
