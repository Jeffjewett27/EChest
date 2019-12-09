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
        public static Version LoadVersion(string hash, string dirpath, DirectoryStructure directory, bool loadData, Changelog changelog = null)
        {
            VersionData data = VersionDataFileManager.LoadTopLevelVD(dirpath, hash, loadData, directory, changelog);

            return new Version(data, hash);
        }
    }
}
