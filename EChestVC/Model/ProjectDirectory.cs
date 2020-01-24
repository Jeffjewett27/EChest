using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.Directory;

namespace EChestVC.Model
{
    class ProjectDirectory
    {
        private DirectoryStructure directory;

        public ProjectDirectory(string path)
        {
            directory = new DirectoryStructure(path);
        }

        /// <summary>
        /// Loads a Changelog
        /// </summary>
        /// <param name="hash">The hash of the changelog to load</param>
        /// <returns></returns>
        public Changelog GetChangelog(string hash)
        {
            return directory.GetChangelog(hash);
        }

        /// <summary>
        /// Saves a Changelog to the file directory
        /// </summary>
        /// <param name="changelog"></param>
        public void CreateChangelog(Changelog changelog)
        {
            directory.CreateChangelog(changelog);
        }

        /// <summary>
        /// Deletes a Changelog from the file directory
        /// </summary>
        /// <param name="changelog"></param>
        public void DeleteChangelog(Changelog changelog)
        {
            directory.DeleteChangelog(changelog);
        }

        /// <summary>
        /// Loads a Commit from the file directory
        /// </summary>
        /// <param name="hash">The hash of the Commit to load</param>
        /// <param name="loader">TODO: add CommitDependencyLoader support</param>
        /// <returns></returns>
        public Commit GetCommit(string hash)
        {
            return directory.GetCommit(hash);
        }

        /// <summary>
        /// Saves a Commit to the file directory
        /// </summary>
        /// <param name="commit"></param>
        public void CreateCommit(Commit commit)
        {
            directory.CreateCommit(commit);
        }

        /// <summary>
        /// Deletes a Commit from the file directory
        /// </summary>
        /// <param name="commit"></param>
        public void DeleteCommit(Commit commit)
        {
            directory.DeleteCommit(commit);
        }

        /// <summary>
        /// Constructs a Version object from the specified hash, including all filenames
        /// </summary>
        /// <param name="hash">The hash identifier of the version</param>
        /// <param name="loadData">If true, will load VersionData objects instead of proxies</param>
        /// <returns></returns>
        public Version GetVersion(string hash, bool loadData = false)
        {
            return directory.GetVersion(hash, loadData);
        }

        /// <summary>
        /// Constructs a Version object from the specified hash, and stores proxies with a changelog dependency
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="changelog"></param>
        /// <returns></returns>
        public Version GetVersion(string hash, Changelog changelog)
        {
            return directory.GetVersion(hash, changelog);
        }

        /// <summary>
        /// Saves a Version to the file directory
        /// </summary>
        /// <param name="version"></param>
        public void CreateVersion(Version version)
        {
            directory.CreateVersion(version);
        }

        /// <summary>
        /// Loads a Version from the Working Directory
        /// </summary>
        /// <returns></returns>
        public Version GetWorkingVersion()
        {
            return directory.GetWorkingVersion();
        }

        /// <summary>
        /// Aggregates files from various Versions into one Version
        /// </summary>
        /// <param name="aggregated"></param>
        /// <returns></returns>
        public Version AggregateVersion(AggregatedChangelog aggregated)
        {
            return directory.AggregateVersion(aggregated);
        }

        public Head GetHead()
        {
            return directory.GetHead();
        }

        public void Initialize()
        {
            directory.Initialize();
        }

        public void UpdateHead(Head head, Commit commit)
        {
            if (head.TargetType != Head.Target.Branch)
            {
                directory.ChangeHead(commit);
            } else
            {
                var branch = head.TargetBranch;
                var newBranch = new Branch(commit, branch.Metadata, branch.Name);
                directory.UpdateBranch(newBranch);
            }
        }

        public void ChangeHead(Commit commit)
        {
            directory.ChangeHead(commit);
        }

        public void ChangeHead(Branch branch)
        {
            directory.ChangeHead(branch);
        }

        public void CreateBranch(Branch branch)
        {
            directory.CreateBranch(branch);
        }

        public void UpdateBranch(Branch branch)
        {
            directory.UpdateBranch(branch);
        }

        public Branch LoadBranch(string name)
        {
            return directory.LoadBranch(name);
        }

        public void DeleteBranch(Branch branch)
        {
            directory.DeleteBranch(branch);
        }
    }
}