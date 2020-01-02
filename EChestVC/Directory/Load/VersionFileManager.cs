using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using EChestVC.Model;
using Version = EChestVC.Model.Version;

namespace EChestVC.Directory.Load
{
    /// <summary>
    /// Methods to interface the Version with the file directory
    /// </summary>
    static class VersionFileManager
    {
        /// <summary>
        /// Loads the Version from the file directory, using the filename as its hash
        /// </summary>
        /// <param name="filename">The name of the Version file to load</param>
        /// <param name="filepath">The absolute path to this Version file</param>
        /// <param name="directory">The directory from which to load dependencies</param>
        /// <param name="loadData">Whether or not to read the files into VersionData</param>
        /// <param name="changelog">An optional Changelog to load the hashes of VersionData</param>
        /// <returns></returns>
        public static Version LoadVersion(string filename, string filepath, DirectoryStructure directory, bool loadData, 
            Changelog changelog = null)
        {
            VersionData data = VersionDataFileManager.LoadTopLevelVD(filepath, filename, loadData, directory, changelog);

            return new Version(data, filename);
        }

        /// <summary>
        /// Loads the Version from the file directory, using VersionData.Hash as its hash
        /// </summary>
        /// <param name="filepath">The absolute path to this Version file</param>
        /// <param name="directory">The directory from which to load dependencies</param>
        /// <param name="loadData">Whether or not to read the files into VersionData</param>
        /// <param name="changelog">An optional Changelog to load the hashes of VersionData</param>
        /// <returns></returns>
        public static Version LoadVersion(string filepath, DirectoryStructure directory, bool loadData, 
            Changelog changelog = null)
        {
            VersionData data = VersionDataFileManager.LoadTopLevelVD(filepath, "", loadData, directory, changelog);

            return new Version(data, data.Hash);
        }

        /// <summary>
        /// Saves a Version into the file directory
        /// </summary>
        /// <param name="directory">The directory in which to save it</param>
        /// <param name="filename">The filename of the Version file</param>
        /// <param name="version">The Version to save</param>
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

        /// <summary>
        /// Aggregates files from various Versions into one Version
        /// </summary>
        /// <param name="aggregated"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static Version AggregateVersion(AggregatedChangelog aggregated, DirectoryStructure directory)
        {
            var versions = new Dictionary<string, Version>();
            VersionBuilder builder = new VersionBuilder();
            Action<Dictionary<string, Tuple<string, Commit>>> build = vals =>
            {
                foreach (var val in vals)
                {
                    string versionHash = val.Value.Item2.Version.Hash;
                    Version v;
                    if (versions.ContainsKey(versionHash))
                    {
                        v = versions[versionHash];
                    }
                    else
                    {
                        v = directory.GetVersion(versionHash);
                        versions.Add(v.Hash, v);
                    }
                    var vdata = v.GetVersionData(val.Key);
                    builder.AddVersionData(val.Key, vdata);
                }
            };
            build(aggregated.Added);
            build(aggregated.Modified);
            foreach (var pair in aggregated.Renamed)
            {
                builder.RenameVersionData(pair.Key, pair.Value);
            }
            return builder.GetVersion();
        }
    }
}
