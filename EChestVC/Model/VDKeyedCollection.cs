using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace EChestVC.Model
{
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

        public string GetHash()
        {
            string hash = "";
            SortedList<string, VersionData> sorted = new SortedList<string, VersionData>(Dictionary);
            foreach (var item in sorted)
            {
                hash += item.Value.Hash;
            }
            return Hash.ComputeHash(hash);
        }
    }
}
