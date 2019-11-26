﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EChestVC.Model
{
    class VersionData
    {
        private readonly StreamReader data;
        private readonly string filename;
        private readonly string hash;

        public virtual StreamReader Data => data;
        public string Filename => filename;
        public string Hash => hash;

        public VersionData(StreamReader data, string filename, string hash)
        {
            this.data = data;
            this.hash = hash;
            this.filename = filename;
        }

        public VersionData(StreamReader data, string filename)
        {
            this.data = data;
            this.hash = GetHash();
            this.filename = filename;
        }

        private string GetHash()
        {
            return Model.Hash.ComputeHash(data.GetHashCode().ToString());
        }
    }
}
