using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.Model;
using Version = EChestVC.Model.Version;

namespace EChestVC.Directory
{
    class CommitProxy : Commit
    {
        private Commit commit;
        private DirectoryStructure directory;

        public override Changelog Changelog
        {
            get
            {
                if (commit == null) Load();
                return commit.Changelog;
            }
        }
        public override CommitMetadata Metadata
        {
            get
            {
                if (commit == null) Load();
                return commit.Metadata;
            }
        }
        public override Commit[] Parents
        {
            get
            {
                if (commit == null) Load();
                return commit.Parents;
            }
        }
        public override Version Version
        {
            get
            {
                if (commit == null) Load();
                return commit.Version;
            }
        }

        public CommitProxy(string hash, DirectoryStructure directory) : base(null, null, null, null, hash)
        {
            this.directory = directory;
        }

        private void Load()
        {
            commit = directory.GetCommit(Hash);
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
