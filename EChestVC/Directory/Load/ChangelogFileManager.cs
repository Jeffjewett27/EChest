using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;
using EChestVC.Model;
using EChestVC.Directory.JSON;

namespace EChestVC.Directory.Load
{
    /// <summary>
    /// Methods to interface the Changelog with the file directory
    /// </summary>
    static class ChangelogFileManager
    {
        /// <summary>
        /// Loads the changelog of stored at the specified file path
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static Changelog LoadChangelog(string filepath)
        {
            string json;
            ChangelogJSON changelog;
            try
            {
                json = File.ReadAllText(filepath);
            }
            catch
            {
                throw new ArgumentException("Could not read file " + filepath);
            }
            changelog = JsonSerializer.Deserialize<ChangelogJSON>(json);
            try
            {
                
            }
            catch
            {
                throw new FormatException("Could not deserialize " + filepath);
            }
            return changelog.GetChangelog();
        }

        /// <summary>
        /// Saves a changelog in the directory with the specified filename
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="filename"></param>
        /// <param name="changelog"></param>
        public static void CreateChangelog(string directory, string filename, Changelog changelog)
        {
            if (changelog == null)
            {
                throw new ArgumentNullException("changelog");
            }
            if (!System.IO.Directory.Exists(directory))
            {
                throw new ArgumentException("Directory " + directory + " does not exist");
            }
            string path = Path.Combine(directory, filename);
            if (File.Exists(path))
            {
                throw new ArgumentException("File " + path + " already exists");
            }
            ChangelogJSON cjson = new ChangelogJSON(changelog);
            string json = JsonSerializer.Serialize<ChangelogJSON>(cjson, JSONFileFormat.GetJsonSerializerOptions());
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// Deletes the Changelog in the directory with the specified filename
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="filename"></param>
        public static void DeleteChangelog(string directory, string filename)
        {
            string path = Path.Combine(directory, filename);
            File.Delete(path);
        }
    }
}
