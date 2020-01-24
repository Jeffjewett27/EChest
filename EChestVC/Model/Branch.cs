using System;
using System.Collections.Generic;
using System.Text;

namespace EChestVC.Model
{
    public class Branch
    {
        private Commit target;
        private BranchMetadata metadata;
        private string name;

        public Commit Target => target;
        public BranchMetadata Metadata => metadata;
        public string Name => name;

        public Branch(Commit target, BranchMetadata metadata, string name)
        {
            this.target = target;
            this.metadata = metadata;
            this.name = name;
        }
    }
}
