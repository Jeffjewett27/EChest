using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using EChestVC.Model;
using EChestVC.HelperFunctions;
using Version = EChestVC.Model.Version;

namespace EChestVC.Directory.Load
{
    static class VersionFileManager
    {
        public static Version LoadVersion(string hash, string dirpath, DirectoryStructure directory, bool loadData, Changelog changelog = null)
        {
            GenericKeyedCollection<string, VersionData> datas = new GenericKeyedCollection<string, VersionData>(p=>p.Filename);

            string[] filepaths = System.IO.Directory.GetFiles(dirpath);
            AddFiles(datas, filepaths, dirpath, VersionData.FileType.File, directory, loadData, changelog);
            string[] subdirpaths = System.IO.Directory.GetDirectories(dirpath, "*", SearchOption.AllDirectories);
            AddFiles(datas, filepaths, dirpath, VersionData.FileType.File, directory, loadData, changelog);

            return new Version(datas, hash);
        }

        private static void AddFiles(GenericKeyedCollection<string, VersionData> datas, string[] filepaths, string dirpath, VersionData.FileType filetype,
            DirectoryStructure directory, bool loadData, Changelog changelog = null)
        {
            foreach (string filepath in filepaths)
            {
                string localPath = GetLocalFilepath(dirpath, filepath);
                VersionData data = loadData ?
                    directory.GetVersionData(localPath) :
                    changelog == null ? new VersionDataProxy(localPath, filetype, directory) :
                        new VersionDataProxy(localPath, filetype, directory, GetFileHash(changelog, localPath));
                datas.Add(data);
            }
        }

        private static string GetFileHash(Changelog changelog, string localpath)
        {
            if (changelog.Added.TryGetValue(localpath, out string hash))
            {
                return hash;
            } else if (changelog.Modified.TryGetValue(localpath, out hash))
            {
                return hash;
            } else
            {
                throw new FormatException("Changelog does not contain hash for file " + localpath);
            }
        }

        private static string GetLocalFilepath(string directory, string abspath)
        {
            Uri directoryUri = new Uri(directory); 
            Uri absUri = new Uri(abspath);
            Uri localUri = directoryUri.MakeRelativeUri(absUri);
            return localUri.ToString();
        }
    }
}
