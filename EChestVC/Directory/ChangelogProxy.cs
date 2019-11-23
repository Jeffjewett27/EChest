using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.Model;

namespace EChestVC.Directory
{
    class ChangelogProxy : Changelog
    {
        private Changelog changelog;
        private DirectoryStructure directory;

        public ChangelogProxy(string hash, DirectoryStructure directory) : base(hash)
        {
            changelog = null;
        }

        public override HashSet<string> Added { get
            {
                if (changelog == null) Load();
                return changelog.Added;
            }
        }

        public override HashSet<string> Modified
        {
            get
            {
                if (changelog == null) Load();
                return changelog.Modified;
            }
        }

        public override HashSet<string> Removed
        {
            get
            {
                if (changelog == null) Load();
                return changelog.Removed;
            }
        }

        public override Dictionary<string, string> Renamed
        {
            get
            {
                if (changelog == null) Load();
                return changelog.Renamed;
            }
        }

        private void Load()
        {
            changelog = directory.GetChangelog(Hash);
        }
    }
}
