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
        public string[] Added { get; }
        public string[] Modified { get; }
        public string[] Removed { get; }
        public string[][] Renamed { get; }

        public ChangelogJSON(Changelog changelog)
        {
            Hash = changelog.Hash;
            Added = changelog.Added.ToArray();
            Modified = changelog.Modified.ToArray();
            Removed = changelog.Removed.ToArray();
            Renamed = (from r in changelog.Renamed select 
                       new string[2] { r.Key, r.Value }
                       ).ToArray();
        }

        public Changelog GetChangelog(DirectoryStructure directory)
        {
            var added = new HashSet<string>(Added);
            var modified = new HashSet<string>(Modified);
            var removed = new HashSet<string>(Removed);
            var renamedVals = from r in Renamed select new KeyValuePair<string, string>(r[0], r[1]);
            var renamed = new Dictionary<string, string>(renamedVals);
            return new Changelog(modified, added, removed, renamed, Hash);
        }
    }
}
