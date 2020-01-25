using System;
using System.Collections.Generic;
using System.Text;

namespace EChestVC.Model
{
    public class BuildMetadata : Metadata
    {
        private string hash;
        public override string Hash => hash;
        public BuildMetadata(DateTime createdTime, string[] authors, string message)
            : base(createdTime, authors, message)
        {
            hash = GenerateHash();
        }

        protected override string GenerateHash()
        {
            return base.GenerateHash();
        }
    }
}
