using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.Model;

namespace EChestVC.Directory.JSON
{
    class BranchJSON
    {
        public string Name { get; set; }
        public string TargetHash { get; set; }
        public BranchMetadataJSON Metadata { get; set; }

        public BranchJSON() { }

        public BranchJSON(Branch branch)
        {
            Name = branch.Name;
            TargetHash = branch.Target.Hash;
            Metadata = new BranchMetadataJSON(branch.Metadata);
        }

        public Branch GetBranch(DirectoryStructure directory)
        {
            if (TargetHash == null)
            {
                throw new FormatException("TargetHash should not be null");
            }
            Commit target = directory.GetCommit(TargetHash);
            if (Metadata == null)
            {
                throw new FormatException("Metadata should not be null");
            }
            BranchMetadata metadata = Metadata.GetMetadata();
            if (Name == null)
            {
                throw new FormatException("Name should not be null");
            }
            return new Branch(target, metadata, Name);
        }

    }
}
