using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.Model;

namespace EChestVC.Directory.JSON
{
    abstract class MetadataJSON
    {
        public string Message { get; set; }
        public string Created { get; set; }
        public string[] Authors { get; set; }

        public MetadataJSON(Metadata metadata)
        {
            Message = metadata.Message;
            Created = DateTimeString.GetString(metadata.CreatedTime);
            Authors = metadata.Authors;
        }

        public MetadataJSON() { }
    }
}
