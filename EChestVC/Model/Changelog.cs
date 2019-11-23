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
        private HashSet<string> modified;
        private HashSet<string> added;
        private HashSet<string> removed;
        private Dictionary<string, string> renamed;
        private string hash;
        private bool isEmpty;

        public virtual HashSet<string> Modified { get => modified; }
        public virtual HashSet<string> Added { get => added; }
        public virtual HashSet<string> Removed { get => removed; }
        public virtual Dictionary<string, string> Renamed { get => renamed; }
        public string Hash { get => hash; }
        public bool IsEmpty { get => isEmpty; }

        public Changelog(HashSet<string> m, HashSet<string> a, HashSet<string> rem, Dictionary<string, string> ren)
        {
            modified = m;
            added = a;
            removed = rem;
            renamed = ren;
            hash = GetHash();
        }

        public Changelog(HashSet<string> m, HashSet<string> a, HashSet<string> rem, Dictionary<string, string> ren, string hash)
        {
            modified = m;
            added = a;
            removed = rem;
            renamed = ren;
            this.hash = hash;
        }

        protected Changelog(string hash)
        {
            modified = new HashSet<string>();
            added = new HashSet<string>();
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
            HashSet<string> mod = new HashSet<string>();
            foreach (string m in one.modified)
            {
                mod.Add(m);
            }
            foreach (string m in two.modified)
            {
                mod.Add(m);
            }
            HashSet<string> add = new HashSet<string>();
            foreach (string a in one.added)
            {
                add.Add(a);
            }
            foreach (string a in two.added)
            {
                add.Add(a);
            }
            HashSet<string> rem = new HashSet<string>();
            foreach (string r in one.removed)
            {
                add.Add(r);
            }
            foreach (string r in two.removed)
            {
                add.Add(r);
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
    }
}
