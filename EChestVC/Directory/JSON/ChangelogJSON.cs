using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using EChestVC.Model;
using System.Linq;
using System.Text.Json.Serialization;

namespace EChestVC.Directory.JSON
{
    /// <summary>
    /// Object to be serialized and deserialized between JSON and Changelog
    /// </summary>
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

        /// <summary>
        /// Converts this object to a Changelog object
        /// </summary>
        /// <returns></returns>
        public Changelog GetChangelog()
        {
            if (Hash == null)
            {
                throw new FormatException("Hash should not be null");
            }
            Dictionary<string, string> added = ConvertToDictionary(Added);
            Dictionary<string, string> modified = ConvertToDictionary(Modified);
            HashSet<string> removed = ConvertToHashSet(Removed);
            Dictionary<string, string> renamed = ConvertToDictionary(Renamed);
            return new Changelog(modified, added, removed, renamed, Hash);
        }

        private Dictionary<string, string> ConvertToDictionary(string[][] values)
        {
            if (values != null)
            {
                var vals = from r in values select new KeyValuePair<string, string>(r[0], r[1]);
                return new Dictionary<string, string>(vals);
            }
            else
            {
                return new Dictionary<string, string>();
            }
        }

        private HashSet<string> ConvertToHashSet(string[] values)
        {
            if (values != null)
            {
                return new HashSet<string>(values);
            }
            else
            {
                return new HashSet<string>();
            }
        }
    }
}
