using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.Model;
using System.Linq;

namespace EChestVC.Directory.JSON
{
    class ChangelogJSON
    {
        public string Hash { get; }
        public string[][] Added { get; }
        public string[][] Modified { get; }
        public string[] Removed { get; }
        public string[][] Renamed { get; }

        public ChangelogJSON(Changelog changelog)
        {
            Hash = changelog.Hash;
            Added = (from r in changelog.Added select 
                        new string[2] { r.Key, r.Value }
                        ).ToArray();
            Modified = (from r in changelog.Modified select
                        new string[2] { r.Key, r.Value }
                        ).ToArray(); ;
            Removed = changelog.Removed.ToArray();
            Renamed = (from r in changelog.Renamed select 
                        new string[2] { r.Key, r.Value }
                        ).ToArray();
        }

        public Changelog GetChangelog(DirectoryStructure directory)
        {
            var addedVals = from r in Added select new KeyValuePair<string, string>(r[0], r[1]);
            var added = new Dictionary<string, string>(addedVals);
            var modifiedVals = from r in Modified select new KeyValuePair<string, string>(r[0], r[1]);
            var modified = new Dictionary<string, string>(modifiedVals);
            var removed = new HashSet<string>(Removed);
            var renamedVals = from r in Renamed select new KeyValuePair<string, string>(r[0], r[1]);
            var renamed = new Dictionary<string, string>(renamedVals);
            return new Changelog(modified, added, removed, renamed, Hash);
        }
    }
}
