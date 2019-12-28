using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.Model;
using Version = EChestVC.Model.Version;

namespace EChestVC.Directory
{
    /// <summary>
    /// A proxy of Version to lazy load Data
    /// </summary>
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

        /// <param name="hash">The hash of this Version</param>
        /// <param name="directory">The directory to load dependencies</param>
        /// <param name="changelog">An optional changelog to load VersionData hashes</param>
        public VersionProxy(string hash, DirectoryStructure directory, Changelog changelog = null) : base(null, hash)
        {
            this.directory = directory;
            this.changelog = changelog;
        }

        /// <summary>
        /// Loads the Version to access Data
        /// </summary>
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
