﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Linq;
using EChestVC.Model;
using EChestVC.Directory.Load;
using Version = EChestVC.Model.Version;

namespace EChestVC.Directory.JSON
{
    class CommitJSON
    {
        public string Hash { get; set; }
        public string[] Parents { get; set; }
        public string Changelog { get; set; }
        public string Version { get; set; }
        public CommitMetadataJSON Metadata { get; set; }

        public CommitJSON(Commit commit)
        {
            Hash = commit.Hash;
            Parents = commit.Parents.Select(p => p.Hash).ToArray();
            Changelog = commit.Changelog.Hash;
            Version = commit.Version.Hash;
            Metadata = new CommitMetadataJSON(commit.Metadata);
        }

        public Commit GetCommit(DirectoryStructure directory, CommitDependencyLoader loader)
        {
            Func<string, Commit> parentFunc = loader.LoadParents ? (Func<string, Commit>)
                (p => directory.GetCommit(p)) : 
                (p => new CommitProxy(p, directory));
            Commit[] parents = Parents.Select(parentFunc).ToArray();

            Changelog changelog = loader.LoadChangelog ?
                directory.GetChangelog(Changelog) :
                new ChangelogProxy(Changelog, directory);

            Version version = loader.LoadVersion ?
                directory.GetVersion(Hash, loader.LoadVersionData) :
                new VersionProxy(Version, directory);

            return new Commit(parents, changelog, version, Metadata.GetMetadata(), Hash);
        }

        public Commit GetCommit(DirectoryStructure directory)
        {
            Commit[] parents = Parents.Select(p => new CommitProxy(p, directory)).ToArray();
            Changelog changelog = new ChangelogProxy(Changelog, directory);
            Version version = new VersionProxy(Version, directory);

            return new Commit(parents, changelog, version, Metadata.GetMetadata(), Hash);
        }
    }
}
