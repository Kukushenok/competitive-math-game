using CompetitiveBackend.Services.Objects;

namespace CompetitiveBackend.Services.ExtraTools
{
    /// <summary>
    /// Логическое разделение: оно обрабатывает только изображения.
    /// </summary>
    public interface IImageProcessor : ILargeFileProcessor
    {
    }
}
