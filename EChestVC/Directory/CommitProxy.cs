using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.Model;

namespace EChestVC.Directory
{
    class CommitProxy : Commit
    {
        private Commit commit;
        private DirectoryStructure directory;

        public override Changelog Changelog => commit.Changelog;
        public override CommitMetadata Metadata => commit.Metadata;
        public override Commit[] Parents => commit.Parents;
        public override VersionData VersionData => commit.VersionData;

        public CommitProxy(string hash, DirectoryStructure directory) : base(null, null, null, null, hash)
        {
            this.directory = directory;
        }

        private void Load()
        {
            directory.GetCommit(Hash);
        }

        public override Changelog DivergentChangelog(Commit other)
        {
            if (commit == null)
            {
                Load();
            }
            return commit.DivergentChangelog(other);
        }
    }
}
