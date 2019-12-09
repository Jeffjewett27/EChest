using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using EChestVC.Model;
using System.Linq;
using System.Text.Json.Serialization;

namespace EChestVC.Directory.JSON
{
    class ChangelogJSON
    {
        public string Hash { get; set; }
        public string[][] Added { get; set; }
        public string[][] Modified { get; set; }
        public string[] Removed { get; set; }
        public string[][] Renamed { get; set; }

        public ChangelogJSON()
        {

        }

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
            Dictionary<string, string> added;
            Dictionary<string, string> modified;
            HashSet<string> removed;
            Dictionary<string, string> renamed;
            if (Added != null)
            {
                var addedVals = from r in Added select new KeyValuePair<string, string>(r[0], r[1]);
                added = new Dictionary<string, string>(addedVals);
            } else
            {
                added = new Dictionary<string, string>();
            }
            if (Modified != null)
            {
                var modifiedVals = from r in Modified select new KeyValuePair<string, string>(r[0], r[1]);
                modified = new Dictionary<string, string>(modifiedVals);
            } else
            {
                modified = new Dictionary<string, string>();
            }
            if (Removed != null)
            {
                removed = new HashSet<string>(Removed);
            } else
            {
                removed = new HashSet<string>();
            }
            if (Renamed != null)
            {
                var renamedVals = from r in Renamed select new KeyValuePair<string, string>(r[0], r[1]);
                renamed = new Dictionary<string, string>(renamedVals);
            } else
            {
                renamed = new Dictionary<string, string>();
            }
            return new Changelog(modified, added, removed, renamed, Hash);
        }
    }
}
