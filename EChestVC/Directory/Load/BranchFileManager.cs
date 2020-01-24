using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;
using EChestVC.Model;
using EChestVC.Directory.JSON;

namespace EChestVC.Directory.Load
{
    static class BranchFileManager
    {
        public static void CreateBranch(string filepath, Branch branch)
        {
            if (File.Exists(filepath))
            {
                throw new ArgumentException(filepath + " already exists");
            }
            UpdateBranch(filepath, branch);
        }

        public static void UpdateBranch(string filepath, Branch branch)
        {
            BranchJSON branchJSON = new BranchJSON(branch);
            var options = JSONFileFormat.GetJsonSerializerOptions();
            string json = JsonSerializer.Serialize(branchJSON, options);
            File.WriteAllText(filepath, json);
        }

        public static Branch LoadBranch(string filepath, DirectoryStructure directory)
        {
            string json;
            try
            {
                json = File.ReadAllText(filepath);
            } catch
            {
                throw new FileNotFoundException(filepath);
            }
            BranchJSON branchJSON = JsonSerializer.Deserialize<BranchJSON>(json);
            return branchJSON.GetBranch(directory);
        }

        public static void DeleteBranch(string filepath)
        {
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            } else
            {
                throw new ArgumentException("There is no Branch " + filepath);
            }
        }
    }
}
