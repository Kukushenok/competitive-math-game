using CompetitiveBackend.Core.Objects;

namespace RepositoriesRealisation.Models
{
    public class ContentDataModel
    {
        public ContentDataModel(byte[] data)
        {
            Data = data;
        }

        public byte[] Data { get; set; }
        public static implicit operator ContentDataModel(LargeData data)
        {
            return new ContentDataModel(data.Data);
        }

        public static implicit operator LargeData(ContentDataModel model)
        {
            return new LargeData(model.Data);
        }
    }
}
