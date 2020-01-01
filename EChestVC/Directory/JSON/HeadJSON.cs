using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.Model;

namespace EChestVC.Directory.JSON
{
    class HeadJSON
    {
        private const string COMMIT = "Commit";
        private const string BRANCH = "Branch";

        public string TargetHash { get; set; }
        public string TargetType { get; set; }

        public HeadJSON() { }

        public HeadJSON(string targetHash, Head.Target targetType)
        {
            TargetHash = targetHash;
            TargetType = targetType switch
            {
            Head.Target.Branch => BRANCH,
            Head.Target.Commit => COMMIT,
            _ => throw new NotImplementedException("HeadJSON not updated with new values")
            };
        }

        /// <summary>
        /// Converts this to a Head object
        /// </summary>
        /// <param name="directory">The directory to load the dependency</param>
        /// <returns></returns>
        public Head GetHead(DirectoryStructure directory)
        {
            if (TargetType == COMMIT)
            {
                Commit commit = directory.GetCommit(TargetHash);
                return new Head(commit);
            } else if (TargetType == BRANCH)
            {
                throw new NotImplementedException("Update to include branch");
            } else
            {
                throw new FormatException("Head must point to either a Commit or a Branch");
            }
        }
    }
}
