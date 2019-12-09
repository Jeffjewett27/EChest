using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EChestVC.Model;

namespace EChestVC.Directory
{
    /// <summary>
    /// Represents a proxy for versiondata with Filetype FileType.File
    /// </summary>
    class VersionDataProxy : VersionData
    {
        private VersionData data;
        private readonly DirectoryStructure directory;
        private bool hasHash;
        private string versionHash;

        public override StreamReader Data
        {
            get
            {
                if (data == null) 
                    Load();
                return data.Data;
            }
        }

        public override string Hash
        {
            get
            {
                if (!hasHash)
                {
                    Load();
                    hasHash = true;
                }
                if (data == null)
                    return base.Hash;
                else
                    return data.Hash;
            }
        }
        
        public static VersionDataProxy Create(string versionHash, string filepath, DirectoryStructure directory)
        {
            return new VersionDataProxy(versionHash, filepath, (StreamReader)null, directory);
        }

        public static VersionDataProxy Create(string versionHash, string filepath, DirectoryStructure directory, string hash)
        {
            return new VersionDataProxy(versionHash, filepath, (StreamReader)null, directory, hash);
        }

        private VersionDataProxy(string versionHash, string filepath, StreamReader data, DirectoryStructure directory) : base(filepath, data, "")
        {
            this.directory = directory;
            this.versionHash = versionHash;
        }

        private VersionDataProxy(string versionHash, string filepath, StreamReader data, DirectoryStructure directory, string hash) : base(filepath, data, hash)
        {
            this.directory = directory;
            hasHash = true;
            this.versionHash = versionHash;
        }

        private void Load()
        {
            data = directory.GetVersionData(versionHash, Filename, true, directory);
        }
    }
}
