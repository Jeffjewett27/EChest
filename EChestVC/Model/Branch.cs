using System;
using System.Collections.Generic;
using System.Text;

namespace EChestVC.Model
{
    public class Branch
    {
        private Commit target;
        private BranchMetadata metadata;
        private string hash;

        public Commit Target => target;
        public BranchMetadata Metadata => metadata;
        public string Hash => hash;

        public Branch(Commit target, BranchMetadata metadata)
        {
            this.target = target;
            this.metadata = metadata;
            hash = GenerateHash();
        }

        public Branch(Commit target, BranchMetadata metadata, string hash)
        {
            this.target = target;
            this.metadata = metadata;
            this.hash = hash;
        }

        public string GenerateHash()
        {
            string hashString = target.Hash + metadata.Hash;
            return Model.Hash.ComputeHash(hashString);
        }
    }
}
