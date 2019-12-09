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
    }
}
