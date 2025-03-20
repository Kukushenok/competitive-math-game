namespace ImageProcessorRealisation
{
    public class DefaultImageConfig : IImageConfig
    {
        public required uint MaxWidth { get; init; }
        public required uint MaxHeight { get; init; }
        public required uint MinWidth { get; init; }
        public required uint MinHeight { get; init; }
    }
}
