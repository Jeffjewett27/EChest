using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.Model;

namespace EChestVC.Directory.JSON
{
    class BranchMetadataJSON
    {
        public string Hash { get; set; }

        public BranchMetadataJSON() { }

        public BranchMetadataJSON(BranchMetadata metadata)
        {
            Hash = metadata.Hash;
        }

        public BranchMetadata GetMetadata()
        {
            return new BranchMetadata();
        }
    }
}
