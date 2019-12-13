using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using EChestVC.Model;
using Version = EChestVC.Model.Version;

namespace EChestVC.Directory.Load
{
    static class VersionFileManager
    {
        public static Version LoadVersion(string filename, string dirpath, DirectoryStructure directory, bool loadData, Changelog changelog = null)
        {
            VersionData data = VersionDataFileManager.LoadTopLevelVD(dirpath, filename, loadData, directory, changelog);

            return new Version(data, filename);
        }

        public static Version LoadVersion(string dirpath, DirectoryStructure directory, bool loadData, Changelog changelog = null)
        {
            VersionData data = VersionDataFileManager.LoadTopLevelVD(dirpath, "", loadData, directory, changelog);

            return new Version(data, data.Hash);
        }

        public static void CreateVersion(string directory, string filename, Version version)
        {
            string path = Path.Combine(directory, filename);
            if (System.IO.Directory.Exists(path))
            {
                throw new IOException("Directory " + path + " already exists");
            }
            System.IO.Directory.CreateDirectory(path);
            foreach (var child in version.Data.Children)
            {
                VersionDataFileManager.CreateVersionData(path, child);
            }
        }
    }
}
