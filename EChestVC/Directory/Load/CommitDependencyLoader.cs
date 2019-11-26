using System;
using System.Collections.Generic;
using System.Text;

namespace EChestVC.Directory.Load
{
    struct CommitDependencyLoader
    {
        private bool loadParents;
        private bool loadChangelog;
        private bool loadVersion;
        private bool loadVersionData;

        public bool LoadParents { get => loadParents; set => loadParents = value; }
        public bool LoadChangelog { get => loadChangelog; set => loadChangelog = value; }
        public bool LoadVersion { get => loadVersion; set => loadVersion = value; }
        public bool LoadVersionData { get => loadVersionData; set => loadVersionData = value; }

        public static CommitDependencyLoader Default()
        {
            return new CommitDependencyLoader();
        }
    }
}
