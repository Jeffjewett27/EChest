using System;
using System.Collections.Generic;
using System.Text;

namespace EChestVC.Model
{
    class Changelog
    {
        public enum Type
        {
            Modified,
            Added,
            Removed,
            Renamed
        }
        private Dictionary<string, string> modified;
        private Dictionary<string, string> added;
        private HashSet<string> removed;
        private Dictionary<string, string> renamed;
        private string hash;
        private bool isEmpty;

        public virtual Dictionary<string, string> Modified { get => modified; }
        public virtual Dictionary<string, string> Added { get => added; }
        public virtual HashSet<string> Removed { get => removed; }
        public virtual Dictionary<string, string> Renamed { get => renamed; }
        public string Hash { get => hash; }
        public bool IsEmpty { get => isEmpty; }

        public Changelog(Dictionary<string, string> m, Dictionary<string, string> a, HashSet<string> rem, Dictionary<string, string> ren)
        {
            modified = m;
            added = a;
            removed = rem;
            renamed = ren;
            hash = GetHash();
        }

        public Changelog(Dictionary<string, string> m, Dictionary<string, string> a, HashSet<string> rem, Dictionary<string, string> ren, string hash)
        {
            modified = m;
            added = a;
            removed = rem;
            renamed = ren;
            this.hash = hash;
        }

        protected Changelog(string hash)
        {
            modified = new Dictionary<string, string>();
            added = new Dictionary<string, string>();
            removed = new HashSet<string>();
            renamed = new Dictionary<string, string>();
            this.hash = hash;
        }

        protected Changelog()
        {
            isEmpty = true;
        }

        public static Changelog EmptyChangelog()
        {
            return new Changelog();
        }

        public static Changelog Copy(Changelog changelog)
        {
            return Add(EmptyChangelog(), changelog);
        }

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

        private string GetHash()
        {
            throw new NotImplementedException();
        }

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
    }
}
