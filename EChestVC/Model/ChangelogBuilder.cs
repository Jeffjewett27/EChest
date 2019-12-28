using System;
using System.Collections.Generic;
using System.Text;

namespace EChestVC.Model
{
    /// <summary>
    /// Builds up a Changelog object (which is an immutable type)
    /// </summary>
    public class ChangelogBuilder
    {
        //Modified: <filename, hash>
        private readonly Dictionary<string, string> modified;
        //Added: <filename, hash>
        private readonly Dictionary<string, string> added;
        //Removed: <filename>
        private readonly HashSet<string> removed;
        //Renamed: <oldName, newName>
        private readonly Dictionary<string, string> renamed;

        public ChangelogBuilder()
        {
            added = new Dictionary<string, string>();
            modified = new Dictionary<string, string>();
            removed = new HashSet<string>();
            renamed = new Dictionary<string, string>();
        }

        /// <summary>
        /// Aggregates with another ChangelogBuilder
        /// </summary>
        /// <param name="other"></param>
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

        /// <summary>
        /// Adds to Added
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="hash"></param>
        public void Add(string filename, string hash)
        {
            added.Add(filename, hash);
        }

        /// <summary>
        /// Adds to Removed
        /// </summary>
        /// <param name="filename"></param>
        public void Remove(string filename)
        {
            removed.Add(filename);
        }

        /// <summary>
        /// Adds to Modified
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="hash"></param>
        public void Modify(string filename, string hash)
        {
            modified.Add(filename, hash);
        }

        /// <summary>
        /// Adds to Renamed
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public void Rename(string oldName, string newName)
        {
            renamed.Add(oldName, newName);
        }

        /// <summary>
        /// Converts this to a Changelog
        /// </summary>
        /// <returns></returns>
        public Changelog GetChangelog()
        {
            return new Changelog(modified, added, removed, renamed);
        }
    }
}
