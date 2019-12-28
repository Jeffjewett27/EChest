using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace EChestVC.Model
{
    /// <summary>
    /// Represents a keyed collection of VersionData, with the key being the filename
    /// </summary>
    public class VDKeyedCollection : KeyedCollection<string, VersionData>
    {
        public VDKeyedCollection()
            : base(null, 0)
        {
            
        }

        protected override string GetKeyForItem(VersionData item)
        {
            return item.Filename;
        }

        /// <summary>
        /// Generates the hash of this collection
        /// </summary>
        /// <returns></returns>
        public string GetHash()
        {
            string hash = "hash";
            SortedList<string, VersionData> sorted = Dictionary != null ? new SortedList<string, VersionData>(Dictionary) :
                new SortedList<string, VersionData>();
            foreach (var item in sorted)
            {
                hash += item.Value.Hash + item.Key;
            }
            return Hash.ComputeHash(hash);
        }
    }
}
