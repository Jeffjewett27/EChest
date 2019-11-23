﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;
using EChestVC.Model;
using EChestVC.Directory.JSON;

namespace EChestVC.Directory
{
    /// <summary>
    /// Manages the file operations within a EChestVC project
    /// </summary>
    class DirectoryStructure
    {
        private const string CHANGELOG_PATH = "Changelogs";
        private const string VERSION_PATH = "Versions";
        private const string COMMIT_PATH = "Commits";
        private const string CHANGELOG_EXT = ".json";
        private const string COMMIT_EXT = ".json";
        private readonly string path;

        public Changelog GetChangelog(string hash)
        {
            string filepath = Path.Combine(path, CHANGELOG_PATH, hash + CHANGELOG_EXT);
            string json;
            ChangelogJSON changelog;
            try
            {
                json = File.ReadAllText(filepath);
            } catch
            {
                throw new ArgumentException("Could not read file " + filepath);
            }
            try
            {
                changelog = JsonSerializer.Deserialize<ChangelogJSON>(json);
            } catch
            {
                throw new FormatException("Could not deserialize " + filepath);
            }
            return changelog.GetChangelog(this);
        }

        public Commit GetCommit(string hash)
        {
            string filepath = Path.Combine(path, COMMIT_PATH, hash + COMMIT_EXT);
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
            try
            {
                commit = JsonSerializer.Deserialize<CommitJSON>(json);
            }
            catch
            {
                throw new FormatException("Could not deserialize " + filepath);
            }
            return commit.GetCommit(this);
        }

        public VersionData GetVersionData(string hash)
        {
            throw new NotImplementedException();
        }
    }
}