using CompetitiveBackend.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
