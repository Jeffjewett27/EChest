using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.Model;
using Version = EChestVC.Model.Version;

namespace EChestVC.Directory
{
    /// <summary>
    /// A proxy of Changelog to lazy load Changelog, Metadata, Parents, and Version
    /// </summary>
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

        /// <param name="hash">This Commit's hash</param>
        /// <param name="directory">The directory to load dependencies</param>
        public CommitProxy(string hash, DirectoryStructure directory) : base((Commit[])null, null, null, null, hash)
        {
            this.directory = directory;
        }

        /// <summary>
        /// Loads the commit to access Changelog, Metadata, Parents, and Version
        /// </summary>
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
