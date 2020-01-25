using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.Model;

namespace EChestVC.Directory.JSON
{
    class BuildMetadataJSON : MetadataJSON
    {

        public BuildMetadataJSON(BuildMetadata metadata) : base(metadata)
        {

        }

        public BuildMetadata GetBuildMetadata()
        {
            var dateCreated = DateTimeString.FromString(Created);
            return new BuildMetadata(dateCreated, Authors, Message);
        }
    }
}
