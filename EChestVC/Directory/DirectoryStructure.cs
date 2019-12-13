using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using EChestVC.Model;
using EChestVC.Directory.Load;
using Version = EChestVC.Model.Version;

namespace EChestVC.Directory
{
    /// <summary>
    /// Manages the file operations within a EChestVC project
    /// </summary>
    public class DirectoryStructure
    {
        private const string CHANGELOG_PATH = "Changelogs";
        private const string VERSION_PATH = "Versions";
        private const string COMMIT_PATH = "Commits";
        private const string CHANGELOG_EXT = ".json";
        private const string COMMIT_EXT = ".json";
        private const string WORKING_PATH = "WorkingDirectory";
        private readonly string path;

        public DirectoryStructure(string path)
        {
            this.path = path;
        }

        public Changelog GetChangelog(string hash)
        {
            string filepath = Path.Combine(path, CHANGELOG_PATH, hash + CHANGELOG_EXT);
            return ChangelogFileManager.LoadChangelog(filepath, this);
        }

        public void CreateChangelog(Changelog changelog)
        {
            if (changelog == null)
            {
                throw new ArgumentNullException("changelog");
            }
            string filename = changelog.Hash + CHANGELOG_EXT;
            string directory = Path.Combine(path, CHANGELOG_PATH);
            ChangelogFileManager.CreateChangelog(directory, filename, changelog);
        }

        public void DeleteChangelog(Changelog changelog)
        {
            if (changelog == null)
            {
                throw new ArgumentNullException("changelog");
            }
            string filename = changelog.Hash + CHANGELOG_EXT;
            string directory = Path.Combine(path, CHANGELOG_PATH);
            ChangelogFileManager.DeleteChangelog(directory, filename);
        }

        public Commit GetCommit(string hash, CommitDependencyLoader loader = null)
        {
            string filepath = Path.Combine(path, COMMIT_PATH, hash + COMMIT_EXT);
            return CommitFileManager.LoadCommit(filepath, this, loader);
        }

        public void CreateCommit(Commit commit)
        {
            if (commit == null)
            {
                throw new ArgumentNullException("commit");
            }
            string filename = commit.Hash + COMMIT_EXT;
            string directory = Path.Combine(path, COMMIT_PATH);
            CommitFileManager.CreateCommit(directory, filename, commit);
        }

        public void DeleteCommit(Commit commit)
        {
            if (commit == null)
            {
                throw new ArgumentNullException("commit");
            }
            string filename = commit.Hash + COMMIT_EXT;
            string directory = Path.Combine(path, COMMIT_PATH);
            CommitFileManager.DeleteCommit(directory, filename);
        }

        public VersionData GetVersionData(string versionHash, string filepath, bool loadData, DirectoryStructure directory, Changelog changelog = null)
        {
            string dirPath = Path.Combine(path, VERSION_PATH, versionHash);
            return VersionDataFileManager.LoadVersionData(dirPath, filepath, loadData, directory, changelog);
        }

        /// <summary>
        /// Constructs a Version object from the specified hash, including all filenames
        /// </summary>
        /// <param name="hash">The hash identifier of the version</param>
        /// <param name="loadData">If true, will load VersionData objects instead of proxies</param>
        /// <returns></returns>
        public Version GetVersion(string hash, bool loadData = false)
        {
            string filepath = Path.Combine(path, VERSION_PATH, hash);
            return VersionFileManager.LoadVersion(hash, filepath, this, loadData);
        }

        /// <summary>
        /// Constructs a Version object from the specified hash, and stores proxies with a changelog dependency
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="changelog"></param>
        /// <returns></returns>
        public Version GetVersion(string hash, Changelog changelog)
        {
            string filepath = Path.Combine(path, VERSION_PATH, hash);
            return VersionFileManager.LoadVersion(hash, filepath, this, false, changelog);
        }

        public void CreateVersion(Version version)
        {
            string directory = Path.Combine(path, VERSION_PATH);
            VersionFileManager.CreateVersion(directory, version.Hash, version);
        }

        public Version GetWorkingVersion()
        {
            string dirpath = Path.Combine(path, WORKING_PATH);
            return VersionFileManager.LoadVersion(dirpath, this, true);
        }
    }
}
