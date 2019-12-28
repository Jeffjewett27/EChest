using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace EChestVC.Model
{
    /// <summary>
    /// Some methods dealing with VersionData path queries
    /// </summary>
    static class VersionDataPath
    {
        /// <summary>
        /// Adds a prefix to a filename (eg. ("prefix", "filename") => "prefix/filename")
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string PrefixFilename(string prefix, string filename)
        {
            return Path.Combine(prefix, filename);
        }

        /// <summary>
        /// Splits a filepath into an array of its directories (eg. ("dir1/dir2/file.txt") => ["dir1", "dir2", "file.txt"])
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static string[] SplitDirectories(string filepath)
        {
            char[] separators =
            {
                Path.DirectorySeparatorChar,
                Path.AltDirectorySeparatorChar
            };
            return filepath.Split(separators);
        }

        /// <summary>
        /// Gets just the filename from a path (eg. ("dir1/dir2/file.txt") => "file.txt")
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static string GetFilename(string filepath)
        {
            return Path.GetFileName(filepath);
        }

        /// <summary>
        /// Gets just the base directory from a path (eg. ("dir1/dir2/file.txt") => "dir1")
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static string GetRootDirectory(string filepath)
        {
            int idx = RootDirectorySeparatorIndex(filepath);
            if (idx == -1)
            {
                idx = filepath.Length;
            }
            return filepath.Substring(0, idx);
        }

        /// <summary>
        /// Returns back the filepath minus the root directory (eg. ("dir1/dir2/file.txt") => "dir2/file.txt")
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static string RemoveRootDirectory(string filepath)
        {
            int idx = RootDirectorySeparatorIndex(filepath);
            if (idx == -1)
            {
                idx = filepath.Length;
            }
            return filepath.Substring(idx + 1);
        }

        /// <summary>
        /// Indexes the first occurrence of a path separator character ("/" or "\")
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
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
