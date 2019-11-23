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

        public override StreamReader Data
        {
            get
            {
                if (data == null) Load();
                return data.Data;
            }
        }

        public VersionDataProxy(string hash, DirectoryStructure directory) : base(null, hash)
        {
        }
        
        private void Load()
        {
            data = directory.GetVersionData(Hash);
        }
    }
}
