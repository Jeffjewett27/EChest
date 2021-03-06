﻿using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.Model;

namespace EChestVC.Directory.JSON
{
    class HeadJSON
    {
        private const string COMMIT = "Commit";
        private const string BRANCH = "Branch";
        private const string UNINITIALIZED = "Uninitialized";

        public string TargetHash { get; set; }
        public string TargetType { get; set; }

        public HeadJSON() { }

        public HeadJSON(Head head)
        {
            TargetHash = head.TargetHash;
            TargetType = head.TargetType switch
            {
            Head.Target.Branch => BRANCH,
            Head.Target.Commit => COMMIT,
            Head.Target.Uninitialized => UNINITIALIZED,
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
                Branch branch = directory.LoadBranch(TargetHash);
                return new Head(branch);
            } else if (TargetType == UNINITIALIZED)
            {
                return new Head();
            } else
            {
                throw new FormatException("Head must point to either a Commit or a Branch");
            }
        }
    }
}
