namespace ImageProcessorRealisation
{
    public class DefaultImageConfig : IImageConfig
    {
        public required uint MinSize { get; init; }
        public required uint MaxSize { get; init; }
    }
}
