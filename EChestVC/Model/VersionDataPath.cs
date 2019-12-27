using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace EChestVC.Model
{
    static class VersionDataPath
    {
        public static string PrefixFilename(string prefix, string filename)
        {
            return Path.Combine(prefix, filename);
        }

        public static string[] SplitDirectories(string filepath)
        {
            char[] separators =
            {
                Path.DirectorySeparatorChar,
                Path.AltDirectorySeparatorChar
            };
            return filepath.Split(separators);
        }

        public static string GetFilename(string filepath)
        {
            return Path.GetFileName(filepath);
        }

        public static string GetRootDirectory(string filepath)
        {
            int idx = RootDirectorySeparatorIndex(filepath);
            if (idx == -1)
            {
                idx = filepath.Length;
            }
            return filepath.Substring(0, idx);
        }

        public static string RemoveRootDirectory(string filepath)
        {
            int idx = RootDirectorySeparatorIndex(filepath);
            if (idx == -1)
            {
                idx = filepath.Length;
            }
            return filepath.Substring(idx + 1);
        }

        private static int RootDirectorySeparatorIndex(string filepath)
        {
            int idx = filepath.IndexOf(Path.DirectorySeparatorChar);
            if (idx == -1)
            {
                idx = filepath.IndexOf(Path.AltDirectorySeparatorChar);
            }
            return idx;
        }
    }
}
