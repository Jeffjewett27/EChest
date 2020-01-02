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
        private const string HEAD_PATH = "HEAD.json";
        private readonly string path;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">The absolute path to the root of the DirectoryStructure</param>
        public DirectoryStructure(string path)
        {
            this.path = path;
        }

        /// <summary>
        /// Loads a Changelog
        /// </summary>
        /// <param name="hash">The hash of the changelog to load</param>
        /// <returns></returns>
        public Changelog GetChangelog(string hash)
        {
            string filepath = Path.Combine(path, CHANGELOG_PATH, hash + CHANGELOG_EXT);
            return ChangelogFileManager.LoadChangelog(filepath);
        }

        /// <summary>
        /// Saves a Changelog to the file directory
        /// </summary>
        /// <param name="changelog"></param>
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

        /// <summary>
        /// Deletes a Changelog from the file directory
        /// </summary>
        /// <param name="changelog"></param>
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

        /// <summary>
        /// Loads a Commit from the file directory
        /// </summary>
        /// <param name="hash">The hash of the Commit to load</param>
        /// <param name="loader">The loading dependencies for the commit</param>
        /// <returns></returns>
        public Commit GetCommit(string hash, CommitDependencyLoader loader = null)
        {
            string filepath = Path.Combine(path, COMMIT_PATH, hash + COMMIT_EXT);
            return CommitFileManager.LoadCommit(filepath, this, loader);
        }

        /// <summary>
        /// Saves a Commit to the file directory
        /// </summary>
        /// <param name="commit"></param>
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

        /// <summary>
        /// Deletes a Commit from the file directory
        /// </summary>
        /// <param name="commit"></param>
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

        /// <summary>
        /// Loads a VersionData from the file directory
        /// </summary>
        /// <param name="versionHash">The hash of the Version the VersionData is contained in</param>
        /// <param name="filepath">The relative path from the Version</param>
        /// <param name="loadData">Whether to read the file</param>
        /// <param name="changelog">An optional parameter to load the hash from the changelog</param>
        /// <returns></returns>
        public VersionData GetVersionData(string versionHash, string filepath, bool loadData, Changelog changelog = null)
        {
            string dirPath = Path.Combine(path, VERSION_PATH, versionHash);
            return VersionDataFileManager.LoadVersionData(dirPath, filepath, loadData, this, changelog);
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

        /// <summary>
        /// Saves a Version to the file directory
        /// </summary>
        /// <param name="version"></param>
        public void CreateVersion(Version version)
        {
            string directory = Path.Combine(path, VERSION_PATH);
            VersionFileManager.CreateVersion(directory, version.Hash, version);
        }

        /// <summary>
        /// Loads a Version from the Working Directory
        /// </summary>
        /// <returns></returns>
        public Version GetWorkingVersion()
        {
            string dirpath = Path.Combine(path, WORKING_PATH);
            return VersionFileManager.LoadVersion(dirpath, this, true);
        }

        /// <summary>
        /// Aggregates files from various Versions into one Version
        /// </summary>
        /// <param name="aggregated"></param>
        /// <returns></returns>
        public Version AggregateVersion(AggregatedChangelog aggregated)
        {
            return VersionFileManager.AggregateVersion(aggregated, this);
        }

        public Head GetHead()
        {
            string filepath = Path.Combine(path, HEAD_PATH);
            return HeadFileManager.LoadHead(filepath, this);
        }

        public void Initialize()
        {
            System.IO.Directory.CreateDirectory(path);
            string[] subdirs = new string[4];
            subdirs[0] = Path.Combine(path, VERSION_PATH);
            subdirs[1] = Path.Combine(path, COMMIT_PATH);
            subdirs[2] = Path.Combine(path, CHANGELOG_PATH);
            subdirs[3] = Path.Combine(path, WORKING_PATH);
            foreach (string dirPath in subdirs)
            {
                System.IO.Directory.CreateDirectory(dirPath);
            }
            string headPath = Path.Combine(path, HEAD_PATH);
            HeadFileManager.CreateHead(headPath);
        }
    }
}
