using System;
using System.Collections.Generic;
using System.Text;

namespace EChestVC.Model
{
    public class Version
    {
        private VersionData data;
        private string hash;

        public string Hash => hash;
        public virtual VersionData Data => data;

        public Version(VersionData data, string hash)
        {
            this.data = data;
            this.hash = hash;
        }

        public Version(VersionData data)
        {
            this.data = data;
            this.hash = GetHash();
        }

        private string GetHash()
        {
            return data.Hash;
        }

        public Changelog GetChangelog(Version child)
        {
            return VersionData.BuildChangelog(Data, child.Data).GetChangelog();
        }
    }
}
