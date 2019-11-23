using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using EChestVC.Model;

namespace EChestVC.Directory
{
    class CommitFile : ICommitFile
    {
        private readonly string directory;
        private readonly string commitHash;
        private readonly string versionHash;
        private readonly string changelogHash;

        public CommitFile(string directory, Commit commit, StreamReader[] files)
        {
            this.directory = directory;
        }

        public Changelog GetChangelog()
        {
            throw new NotImplementedException();
        }

        public Commit GetCommit()
        {
            throw new NotImplementedException();
        }

        public string GetCommitHash()
        {
            throw new NotImplementedException();
        }

        public StreamReader GetFile(string relativePath)
        {
            throw new NotImplementedException();
        }

        public StreamReader[] GetFiles(string[] relativePaths)
        {
            throw new NotImplementedException();
        }
    }
}
