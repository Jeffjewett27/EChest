using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EChestVC.Model;

namespace EChestVC.Directory
{
    class VersionDataProxy : VersionData
    {
        private VersionData data;
        private DirectoryStructure directory;
        private bool hasHash;

        public override StreamReader Data
        {
            get
            {
                if (data == null) Load();
                return data.Data;
            }
        }

        public override string Hash
        {
            get
            {
                if (!hasHash) Load();
                if (data == null)
                    return base.Hash;
                else
                    return data.Hash;
            }
        }

        public VersionDataProxy(string filepath, FileType filetype, DirectoryStructure directory) : base(null, filepath, filetype)
        {
            this.directory = directory;
        }

        public VersionDataProxy(string filepath, FileType filetype, DirectoryStructure directory, string hash) : base(null, filepath, filetype, hash)
        {
            this.directory = directory;
            hasHash = true;
        }

        private void Load()
        {
            data = directory.GetVersionData(Hash);
        }
    }
}
