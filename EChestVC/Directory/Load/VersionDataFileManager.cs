using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using EChestVC.Model;

namespace EChestVC.Directory.Load
{
    static class VersionDataFileManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirpath">The absolute path to the directory in which all files are contained</param>
        /// <param name="filepath">The relative path to a file or directory from dirpath</param>
        /// <param name="loadData">Whether or not to read the files</param>
        /// <param name="directory">The content directory from which to read</param>
        /// <param name="changelog">An optional Changelog parameter to fill in hashes</param>
        /// <returns></returns>
        public static VersionData LoadVersionData(string dirpath, string filepath, bool loadData, DirectoryStructure directory, Changelog changelog = null)
        {
            string path = Path.Combine(dirpath, filepath);
            string filename = Path.GetFileName(path);
            if (!File.Exists(path) && !System.IO.Directory.Exists(path))
            {
                throw new ArgumentException(path + " does not exist");
            }
            FileAttributes fileattributes = File.GetAttributes(path);
            if (fileattributes.HasFlag(FileAttributes.Directory))
            {
                return LoadDirectory(dirpath, filepath, filename, loadData, directory, changelog);
            }
            else
            {
                return LoadFile(path, dirpath, filepath, loadData, directory, changelog);
            }
        }

        private static VersionData LoadDirectory(string dirPath, string filePath, string filename, bool loadData, DirectoryStructure directory, 
            Changelog changelog = null)
        {
            string path = Path.Combine(dirPath, filePath);
            VDKeyedCollection datas = GetVersionDatas(path, dirPath, filePath, loadData, directory, changelog);
            if (changelog == null)
            {
                return new VersionData(filename, datas);
            }
            else
            {
                string hash = changelog.GetCachedHash(filePath);
                return new VersionData(filename, datas, hash);
            }
        }

        private static VDKeyedCollection GetVersionDatas(string path, string dirPath, string filePath, bool loadData, DirectoryStructure directory, Changelog changelog)
        {
            VDKeyedCollection datas = new VDKeyedCollection();
            IEnumerable<string> directoryNames = System.IO.Directory.EnumerateDirectories(path).Select(Path.GetFileName);
            IEnumerable<string> fileNames = System.IO.Directory.EnumerateFiles(path).Select(Path.GetFileName);
            foreach (string dir in directoryNames)
            {
                string dp = Path.Combine(filePath, dir);
                datas.Add(LoadVersionData(dirPath, dp, loadData, directory, changelog));
            }
            foreach (string file in fileNames)
            {
                string fp = Path.Combine(filePath, file);
                datas.Add(LoadVersionData(dirPath, fp, loadData, directory, changelog));
            }
            return datas;
        }

        private static VersionData LoadFile(string path, string dirPath, string filepath, bool loadData, DirectoryStructure directory, Changelog changelog)
        {
            string filename = Path.GetFileName(path);
            if (loadData)
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                if (changelog == null)
                {
                    return new VersionData(filename, fs);
                }
                else
                {
                    string hash = changelog.GetCachedHash(filepath);
                    return new VersionData(filename, fs, hash);
                }
            }
            else
            {
                string versionHash = Path.GetFileName(dirPath);
                if (changelog == null)
                {
                    return VersionDataProxy.Create(versionHash, filepath, filename, directory);
                }
                else
                {
                    string hash = changelog.GetCachedHash(filepath);
                    return VersionDataProxy.Create(versionHash, filepath, filename, directory, hash);
                }
            }
        }

        public static VersionData LoadTopLevelVD(string dirPath, string versionHash, bool loadData, DirectoryStructure directory, Changelog changelog = null)
        {
            VDKeyedCollection datas = GetVersionDatas(dirPath, dirPath, "", loadData, directory, changelog);
            return new VersionData(versionHash, datas);
        }

        public static void CreateVersionData(string directory, VersionData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (!System.IO.Directory.Exists(directory))
            {
                throw new IOException("Directory " + directory + " does not exist");
            }
            string path = Path.Combine(directory, data.Filename);
            if (data.Filetype == VersionData.FileType.Directory)
            {
                System.IO.Directory.CreateDirectory(path);
                foreach (VersionData vd in data.Children)
                {
                    CreateVersionData(path, vd);
                }
            }
            else
            {
                using var fs = File.Create(path);
                data.Data.CopyTo(fs);
            }
        }
    }


}
