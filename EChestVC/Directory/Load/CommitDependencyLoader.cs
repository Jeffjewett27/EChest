using System;
using System.Collections.Generic;
using System.Text;

namespace EChestVC.Directory.Load
{
    /// <summary>
    /// An object containing information on which dependencies a commit should be loaded with
    /// Is a WIP
    /// </summary>
    public class CommitDependencyLoader
    {
        private const string defaultKey = "default";

        private bool loadParents;
        private bool loadChangelog;
        private bool loadVersion;
        private bool loadVersionData;
        private Dictionary<string, CommitDependencyLoader> parents;

        public bool LoadParents { get => loadParents; set => loadParents = value; }
        public bool LoadChangelog { get => loadChangelog; set => loadChangelog = value; }
        public bool LoadVersion { get => loadVersion; set => loadVersion = value; }
        public bool LoadVersionData { get => loadVersionData; set => loadVersionData = value; }

        public static CommitDependencyLoader Default()
        {
            return new CommitDependencyLoader();
        }

        public CommitDependencyLoader GetParent(string hash)
        {
            if (parents.TryGetValue(hash, out CommitDependencyLoader parent))
            {
                return parent;
            } else if (parents.TryGetValue(defaultKey, out parent))
            {
                return parent;
            } else
            {
                return Default();
            }
        }

        public bool ShouldLoadParent(string hash)
        {
            if (hash == defaultKey)
            {
                return false;
            }
            return parents.ContainsKey(hash);
        }
    }
}
