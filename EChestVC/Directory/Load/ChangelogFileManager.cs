using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;
using EChestVC.Model;
using EChestVC.Directory.JSON;

namespace EChestVC.Directory.Load
{
    static class ChangelogFileManager
    {
        public static Changelog LoadChangelog(string filepath, DirectoryStructure directory)
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
            return changelog.GetChangelog(directory);
        }

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

        public static void DeleteChangelog(string directory, string filename)
        {
            string path = Path.Combine(directory, filename);
            File.Delete(path);
        }
    }
}
