using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EChestVC.Model
{
    class VersionData
    {
        public enum FileType
        {
            File,
            Directory
        }

        private readonly FileType filetype;
        private readonly StreamReader data;
        private readonly string filename;
        private readonly string hash;

        public virtual StreamReader Data => data;
        public string Filename => filename;
        public virtual string Hash => hash;

        public VersionData(StreamReader data, string filename, FileType filetype, string hash)
        {
            this.data = data;
            this.hash = hash;
            this.filename = filename;
            this.filetype = filetype;
        }

        public VersionData(StreamReader data,  string filename, FileType filetype)
        {
            this.data = data;
            this.hash = GetHash();
            this.filename = filename;
            this.filetype = filetype;
        }

        private string GetHash()
        {
            return Model.Hash.ComputeHash(data.GetHashCode().ToString());
        }
    }
}
