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
        private readonly VDKeyedCollection children;
        private readonly string filename;
        private readonly string hash;

        public virtual StreamReader Data => data;
        public virtual VDKeyedCollection Children => children;
        public string Filename => filename;
        public virtual string Hash => hash;
        public FileType Filetype => filetype;

        public VersionData(string filename, StreamReader data, string hash)
        {
            this.data = data;
            this.hash = hash;
            this.filename = filename;
            this.filetype = FileType.File;
        }

        public VersionData(string filename, StreamReader data)
        {
            this.data = data;
            this.filename = filename;
            this.filetype = FileType.File;
            this.hash = GetHash();
        }

        public VersionData(string filename, VDKeyedCollection children, string hash)
        {
            this.filename = filename;
            this.children = children;
            this.hash = hash;
        }

        public VersionData(string filename, VDKeyedCollection children)
        {
            this.filename = filename;
            this.children = children;
            this.filetype = FileType.Directory;
            this.hash = GetHash();
        }

        private string GetHash()
        {
            if (filetype == FileType.Directory)
            {
                return children.GetHash();
            } else
            {
                return Model.Hash.ComputeHash(data.GetHashCode().ToString());
            }
        }

        public VersionData PathGetFile(string path)
        {
            //path example: thisfilename/childfilename/morestuff
            char separator = Path.DirectorySeparatorChar;
            int idx = path.IndexOf(separator);
            if (idx < 0)
            {
                if (filename == path)
                {
                    return this;
                } else
                {
                    throw new ArgumentException("file or directory " + path + " not found");
                }
            }
            if (filetype == FileType.File)
            {
                throw new ArgumentException(filename + " is not a directory for " + path);
            }
            string childPath = path.Substring(idx + 1); //childPath example: childfilename/morestuff
            string childName = childPath.Split(separator)[0]; //childName example: childfilename
            if (children.TryGetValue(childName, out VersionData child))
            {
                return child.PathGetFile(childPath);
            } else
            {
                throw new ArgumentException("");
            }
        }
    }
}
