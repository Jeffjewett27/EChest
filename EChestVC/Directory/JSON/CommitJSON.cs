using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using EChestVC.Model;
using System.Linq;

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
            Version = commit.VersionData.Hash;
            Metadata = new CommitMetadataJSON(commit.Metadata);
        }

        public Commit GetCommit(DirectoryStructure directory, bool loadParents = false, bool loadChangelog = false, bool loadVersion = false)
        {
            Func<string, Commit> parentFunc = loadParents ? (Func<string, Commit>)
                (p => directory.GetCommit(p)) : 
                (p => new CommitProxy(p, directory));
            Commit[] parents = Parents.Select(parentFunc).ToArray();

            Changelog changelog = loadChangelog ?
                directory.GetChangelog(Changelog) :
                new ChangelogProxy(Changelog, directory);

            VersionData version = loadVersion ?
                directory.GetVersionData(Hash) :
                new VersionDataProxy(Version, directory);

            return new Commit(parents, changelog, version, Metadata.GetMetadata(), Hash);
        }
    }
}
