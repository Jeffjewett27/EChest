using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EChestVC.Model
{
    class VersionData
    {
        private readonly StreamReader data;
        private readonly string filename;
        private readonly string hash;

        public virtual StreamReader Data => data;
        public string Filename => filename;
        public string Hash => hash;

        public VersionData(StreamReader data, string hash)
        {
            this.data = data;
            this.hash = hash;
        }

        private string GetHash()
        {
            return Model.Hash.ComputeHash(data.GetHashCode().ToString());
        }
    }
}
