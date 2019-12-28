using System;
using System.Collections.Generic;
using System.Text;

namespace EChestVC.Model
{
    /// <summary>
    /// Stores information about which files have been Added, Modified, Removed, or Renamed
    /// </summary>
    public class Changelog
    {
        public enum Type
        {
            Modified,
            Added,
            Removed,
            Renamed
        }
        //Modified: <filename, hash>
        private readonly Dictionary<string, string> modified;
        //Added: <filename, hash>
        private readonly Dictionary<string, string> added;
        //Removed: <filename>
        private readonly HashSet<string> removed;
        //Renamed: <oldName, newName>
        private readonly Dictionary<string, string> renamed;
        private string hash;

        /// <summary>
        /// A collection of <filename, hash> pairs for modified files
        /// </summary>
        public virtual Dictionary<string, string> Modified { get => modified; }
        /// <summary>
        /// A collection of <filename, hash> pairs for added files
        /// </summary>
        public virtual Dictionary<string, string> Added { get => added; }
        /// <summary>
        /// A collection of filenames for removed files
        /// </summary>
        public virtual HashSet<string> Removed { get => removed; }
        /// <summary>
        /// A collection of <oldName, newName> pairs for renamed files
        /// </summary>
        public virtual Dictionary<string, string> Renamed { get => renamed; }
        public string Hash { get => hash; }

        /// <param name="modified">A collection of <filename, hash> pairs for modified files</param>
        /// <param name="added">A collection of <filename, hash> pairs for added files</param>
        /// <param name="removed">A collection of filenames for removed files</param>
        /// <param name="renamed">A collection of <oldName, newName> pairs for renamed files</param>
        public Changelog(Dictionary<string, string> modified, Dictionary<string, string> added, HashSet<string> removed, 
            Dictionary<string, string> renamed)
        {
            this.modified = modified;
            this.added = added;
            this.removed = removed;
            this.renamed = renamed;
            hash = GetHash();
        }

        /// <param name="modified">A collection of <filename, hash> pairs for modified files</param>
        /// <param name="added">A collection of <filename, hash> pairs for added files</param>
        /// <param name="removed">A collection of filenames for removed files</param>
        /// <param name="renamed">A collection of <oldName, newName> pairs for renamed files</param>
        /// <param name="hash">The Changelog's hash</param>
        public Changelog(Dictionary<string, string> modified, Dictionary<string, string> added, HashSet<string> removed,
            Dictionary<string, string> renamed, string hash)
        {
            this.modified = modified;
            this.added = added;
            this.removed = removed;
            this.renamed = renamed;
            this.hash = hash;
        }

        /// <param name="hash">The Changelog's hash</param>
        public Changelog(string hash)
        {
            modified = new Dictionary<string, string>();
            added = new Dictionary<string, string>();
            removed = new HashSet<string>();
            renamed = new Dictionary<string, string>();
            this.hash = hash;
        }

        public Changelog()
        {
            modified = new Dictionary<string, string>();
            added = new Dictionary<string, string>();
            removed = new HashSet<string>();
            renamed = new Dictionary<string, string>();
            this.hash = GetHash();
        }

        public static Changelog EmptyChangelog()
        {
            return new Changelog();
        }

        public static Changelog Copy(Changelog changelog)
        {
            return Add(EmptyChangelog(), changelog);
        }

        /// <summary>
        /// Combines two Changelogs
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public static Changelog Add(Changelog one, Changelog two)
        {
            Dictionary<string, string> mod = new Dictionary<string, string>();
            foreach (var m in one.modified)
            {
                mod.Add(m.Key, m.Value);
            }
            foreach (var m in two.modified)
            {
                mod.Add(m.Key, m.Value);
            }
            Dictionary<string, string> add = new Dictionary<string, string>();
            foreach (var a in one.added)
            {
                add.Add(a.Key, a.Value);
            }
            foreach (var a in two.added)
            {
                add.Add(a.Key, a.Value);
            }
            HashSet<string> rem = new HashSet<string>();
            foreach (string r in one.removed)
            {
                rem.Add(r);
            }
            foreach (string r in two.removed)
            {
                rem.Add(r);
            }
            Dictionary<string, string> ren = new Dictionary<string, string>();
            foreach (var keyValue in one.renamed)
            {
                ren.Add(keyValue.Key, keyValue.Value);
            }
            foreach (var keyValue in two.renamed)
            {
                ren.Add(keyValue.Key, keyValue.Value);
            }
            return new Changelog(mod, add, rem, ren);
        }

        /// <summary>
        /// Gets the Changelog's hash
        /// </summary>
        /// <returns></returns>
        private string GetHash()
        {
            StringBuilder sb = new StringBuilder("changelog");
            foreach (var a in added)
            {
                sb.Append(a.Key);
                sb.Append(a.Value);
            }
            foreach (var m in modified)
            {
                sb.Append(m.Key);
                sb.Append(m.Value);
            }
            foreach (var r in removed)
            {
                sb.Append(r);
            }
            foreach (var r in renamed)
            {
                sb.Append(r.Key);
                sb.Append(r.Value);
            }
            return Model.Hash.ComputeHash(sb.ToString());
        }

        /// <summary>
        /// Gets the hash value from the filepath key in Added or Modified
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public virtual string GetCachedHash(string filepath)
        {
            if (Added.TryGetValue(filepath, out string hash))
            {
                return hash;
            } else if (Modified.TryGetValue(filepath, out hash))
            {
                return hash;
            } else
            {
                throw new ArgumentException("Path " + filepath + " is not cached");
            }
        }

        public virtual bool IsEmpty()
        {
            int total = added.Count + modified.Count + removed.Count + renamed.Count;
            return total == 0;
        }
    }
}
