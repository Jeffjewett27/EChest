using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.Model;
using EChestVC.HelperFunctions;
using Version = EChestVC.Model.Version;

namespace EChestVC.Directory
{
    class VersionProxy : Version
    {
        private Version version;
        private DirectoryStructure directory;
        private Changelog changelog;

        public override GenericKeyedCollection<string, VersionData> Files
        {
            get
            {
                if (version == null) Load();
                return version.Files;
            }
        }

        public VersionProxy(string hash, DirectoryStructure directory, Changelog changelog = null) : base(null, hash)
        {
            this.directory = directory;
            this.changelog = changelog;
        }

        private void Load()
        {
            version = directory.GetVersion(Hash);
        }
    }
}
