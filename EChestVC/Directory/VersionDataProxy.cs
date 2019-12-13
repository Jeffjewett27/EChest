﻿using System;
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

        public override Stream Data
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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="versionHash">The identifier of the version the data is contained in</param>
        /// <param name="filepath">The relative path from the top level version folder</param>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static VersionDataProxy Create(string versionHash, string filepath, DirectoryStructure directory)
        {
            return new VersionDataProxy(versionHash, filepath, (Stream)null, directory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="versionHash">The identifier of the version the data is contained in</param>
        /// <param name="filepath">The relative path from the top level version folder</param>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static VersionDataProxy Create(string versionHash, string filepath, DirectoryStructure directory, string hash)
        {
            return new VersionDataProxy(versionHash, filepath, (Stream)null, directory, hash);
        }

        private VersionDataProxy(string versionHash, string filepath, Stream data, DirectoryStructure directory) : base(filepath, data, "")
        {
            this.directory = directory;
            this.versionHash = versionHash;
        }

        private VersionDataProxy(string versionHash, string filepath, Stream data, DirectoryStructure directory, string hash) : base(filepath, data, hash)
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
