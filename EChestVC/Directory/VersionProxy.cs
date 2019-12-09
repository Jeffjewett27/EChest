using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.Model;
using Version = EChestVC.Model.Version;

namespace EChestVC.Directory
{
    class VersionProxy : Version
    {
        private Version version;
        private DirectoryStructure directory;
        private Changelog changelog;

        public override VersionData Data
        {
            get
            {
                if (version == null) Load();
                return version.Data;
            }
        }

        public VersionProxy(string hash, DirectoryStructure directory, Changelog changelog = null) : base(null, hash)
        {
            this.directory = directory;
            this.changelog = changelog;
        }

        private void Load()
        {
            if (changelog == null)
            {
                version = directory.GetVersion(Hash);
            } else
            {
                version = directory.GetVersion(Hash, changelog);
            }
        }
    }
}
