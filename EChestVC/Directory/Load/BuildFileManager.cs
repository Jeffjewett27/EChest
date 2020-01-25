using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;
using EChestVC.Model;
using EChestVC.Directory.JSON;

namespace EChestVC.Directory.Load
{
    static class BuildFileManager
    {
        public static Build LoadBuild(string filepath, DirectoryStructure directory)
        {
            if (!File.Exists(filepath))
            {
                throw new FileNotFoundException(filepath + " does not exist");
            }
            string json = File.ReadAllText(filepath);
            BuildJSON branchJSON = JsonSerializer.Deserialize<BuildJSON>(json);
            return branchJSON.GetBuild(directory);
        }

        public static void CreateBuild(string filepath, Build build)
        {
            if (File.Exists(filepath))
            {
                throw new ArgumentException(filepath + " already exists");
            }
            var buildJSON = new BuildJSON(build);
            var options = JSONFileFormat.GetJsonSerializerOptions();
            string json = JsonSerializer.Serialize(buildJSON, options);
            File.WriteAllText(filepath, json);
        }

        public static void DeleteBuild(string filepath)
        {
            if (!File.Exists(filepath))
            {
                throw new FileNotFoundException(filepath + " cannot be deleted");
            }
            File.Delete(filepath);
        }
    }
}
