using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.HelperFunctions;

namespace EChestVC.Model
{
    class Version
    {
        private GenericKeyedCollection<string, VersionData> files;
        private string hash;

        public string Hash => hash;
        public virtual GenericKeyedCollection<string, VersionData> Files => files;

        public Version(GenericKeyedCollection<string, VersionData> files, string hash)
        {
            this.files = files;
            this.hash = hash;
        }

        public Version(GenericKeyedCollection<string, VersionData> files)
        {
            this.files = files;
            this.hash = GetHash();
        }

        private string GetHash()
        {
            return Model.Hash.ComputeHash(files.GetHashCode().ToString());
        }
    }
}
