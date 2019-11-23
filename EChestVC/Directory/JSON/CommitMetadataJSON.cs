using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.Model;

namespace EChestVC.Directory.JSON
{
    class CommitMetadataJSON
    {
        public CommitMetadataJSON(CommitMetadata metadata)
        {
        }

        public CommitMetadata GetMetadata()
        {
            return new CommitMetadata();
        }
    }
}
