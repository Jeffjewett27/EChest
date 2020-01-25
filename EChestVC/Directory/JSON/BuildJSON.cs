using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.Model;

namespace EChestVC.Directory.JSON
{
    class BuildJSON
    {
        public string Hash { get; set; }
        public string Parent { get; set; }
        public string Changelog { get; set; }
        public string Version { get; set; }
        public BuildMetadataJSON Metadata { get; set; }

        public BuildJSON(Build build)
        {
            Hash = build.Hash;
            Parent = build.Parents.Length > 0 ? build.Parents[0].Hash : null;
            Changelog = build.Changelog.Hash;
            Version = build.Version.Hash;
            Metadata = new BuildMetadataJSON((BuildMetadata)build.Metadata);
        }

        public BuildJSON() { }

        public Build GetBuild(DirectoryStructure directory)
        {
            var parent = directory.GetBuild(Parent);
            var changelog = directory.GetChangelog(Changelog);
            var version = directory.GetVersion(Version);
            var metadata = Metadata.GetBuildMetadata();
            return new Build(parent, changelog, version, metadata);
        }
    }
}
