namespace ImageProcessorRealisation
{
    public interface IImageConfig
    {
        public uint MaxWidth { get; }
        public uint MaxHeight { get; }
        public uint MinWidth { get; }
        public uint MinHeight { get; }
    }
}
