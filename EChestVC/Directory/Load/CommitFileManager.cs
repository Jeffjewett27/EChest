using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;
using EChestVC.Model;
using EChestVC.Directory.JSON;

namespace EChestVC.Directory.Load
{
    static class CommitFileManager
    {
        public static Commit LoadCommit(string filepath, DirectoryStructure directory, CommitDependencyLoader loader)
        {
            string json;
            CommitJSON commit;
            try
            {
                json = File.ReadAllText(filepath);
            }
            catch
            {
                throw new ArgumentException("Could not read file " + filepath);
            }
            commit = JsonSerializer.Deserialize<CommitJSON>(json);
            if (commit == null)
                throw new ArgumentNullException("commitjson is null");
            if (loader != null)
            {
                return commit.GetCommit(directory, loader);
            }
            else
            {
                return commit.GetCommit(directory);
            }
        }
    }
}
