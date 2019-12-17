using System;
using System.Collections.Generic;
using System.Text;

namespace EChestVC.Model
{
    public class ChangelogBuilder
    {
        private Dictionary<string, string> added;
        private Dictionary<string, string> modified;
        private HashSet<string> removed;
        private Dictionary<string, string> renamed;

        public ChangelogBuilder()
        {
            added = new Dictionary<string, string>();
            modified = new Dictionary<string, string>();
            removed = new HashSet<string>();
            renamed = new Dictionary<string, string>();
        }

        public void Aggregate(ChangelogBuilder other)
        {
            foreach (var a in other.added)
            {
                added.Add(a.Key, a.Value);
            }
            foreach (var m in other.modified)
            {
                modified.Add(m.Key, m.Value);
            }
            foreach (var r in other.removed)
            {
                removed.Add(r);
            }
            foreach (var r in other.renamed)
            {
                renamed.Add(r.Key, r.Value);
            }
        }

        public void Add(string filename, string hash)
        {
            added.Add(filename, hash);
        }

        public void Remove(string filename)
        {
            removed.Add(filename);
        }

        public void Modify(string filename, string hash)
        {
            modified.Add(filename, hash);
        }

        public void Rename(string oldName, string newName)
        {
            renamed.Add(oldName, newName);
        }

        public Changelog GetChangelog()
        {
            return new Changelog(modified, added, removed, renamed);
        }
    }
}
