using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.Model;
using EChestVC.Directory;
using Version = EChestVC.Model.Version;

namespace EChestVC.ViewModel
{
    /// <summary>
    /// A command to create a commit
    /// </summary>
    public class CommitCommand
    {
        private DirectoryStructure directory;

        /// <param name="path">The path of the DirectoryStructure</param>
        public CommitCommand(string path)
        {
            directory = new DirectoryStructure(path);
        }

        public void Create(string message)
        {
            //Loads the files in the working directory
            Version working = directory.GetWorkingVersion();
            //Aggregates the files from all commits up to HEAD
            Head head = directory.GetHead();
            Commit parentCommit = head.GetTarget();
            AggregatedChangelog parentAggregate = parentCommit.AggregateChangelog();
            Version parent = directory.AggregateVersion(parentAggregate);
            //Creates the data fields for commit
            Changelog diffChangelog = parent.GetChangelog(working);
            Version diffVersion = working.GetChangelogVersion(diffChangelog);
            CommitMetadata metadata = new CommitMetadata(message);
            //Creates the commit
            Commit newCommit = new Commit(parentCommit, diffChangelog, diffVersion, metadata);
            //Adds the data fields to the file directory
            directory.CreateVersion(diffVersion);
            directory.CreateChangelog(diffChangelog);
            directory.CreateCommit(newCommit);
        }
    }
}
