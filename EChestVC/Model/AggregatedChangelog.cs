using System;
using System.Collections.Generic;
using System.Text;

namespace EChestVC.Model
{
    /// <summary>
    /// Represents an aggregation of changelogs down a branch of commits. Stores information about which commits files are located in.
    /// </summary>
    public class AggregatedChangelog
    {
        private Dictionary<string, Tuple<string, string>> modified; //file-><datahash, versionhash>
        private Dictionary<string, Tuple<string, string>> added; //file->hash
        private HashSet<string> removed; //file
        private Dictionary<string, string> renamed; //new file-> old file

        public Dictionary<string, Tuple<string, string>> Modified => modified;
        public Dictionary<string, Tuple<string, string>> Added => added;
        public HashSet<string> Removed => removed;
        public Dictionary<string, string> Renamed => renamed; 

        public AggregatedChangelog(string hash, Changelog changelog)
        {
            modified = new Dictionary<string, Tuple<string, string>>();
            added = new Dictionary<string, Tuple<string, string>>();
            removed = new HashSet<string>();
            renamed = new Dictionary<string, string>();

            AggregateNext(hash, changelog);
        }

        private AggregatedChangelog()
        {
            modified = new Dictionary<string, Tuple<string, string>>();
            added = new Dictionary<string, Tuple<string, string>>();
            removed = new HashSet<string>();
            renamed = new Dictionary<string, string>();
        }

        public static AggregatedChangelog EmptyChangelog()
        {
            return new AggregatedChangelog();
        }

        /// <summary>
        /// Adds the next changelog to the AggregatedChangelog, keeping track of the file locations
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="changelog"></param>
        public void AggregateNext(string hash, Changelog changelog)
        {
            AggregateAdd(hash, changelog);

            AggregateMod(hash, changelog);

            AggregateRem(hash, changelog);

            AggregateRen(hash, changelog);
        }

        /// <summary>
        /// Aggregates all of changelog.Added
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="changelog"></param>
        private void AggregateAdd(string hash, Changelog changelog)
        {
            foreach (var add in changelog.Added)
            {
                if (added.ContainsKey(add.Key))
                {
                    added[add.Key] = Tuple.Create(add.Value, hash);
                }
                else if (removed.Contains(add.Key))
                {
                    modified.Add(add.Key, Tuple.Create(add.Value, hash));
                    removed.Remove(add.Key);
                }
                else if (modified.ContainsKey(add.Key))
                {
                    throw new FormatException("Attempted to add an existing and modified file");
                }
                else
                {
                    added.Add(add.Key, Tuple.Create(add.Value, hash));
                }
            }
        }

        /// <summary>
        /// Aggregates all of changelog.Modified
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="changelog"></param>
        private void AggregateMod(string hash, Changelog changelog)
        {
            foreach (var mod in changelog.Modified)
            {
                if (added.ContainsKey(mod.Key))
                {
                    added[mod.Key] = Tuple.Create(mod.Value, hash);
                }
                else if (removed.Contains(mod.Key))
                {
                    throw new FormatException("Attempted to modify a removed file");
                }
                else if (modified.ContainsKey(mod.Key))
                {
                    modified[mod.Key] = Tuple.Create(mod.Value, hash);
                }
                else
                {
                    modified.Add(mod.Key, Tuple.Create(mod.Value, hash));
                }
            }
        }

        /// <summary>
        /// Aggregates all of changelog.Removed
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="changelog"></param>
        private void AggregateRem(string hash, Changelog changelog)
        {
            foreach (var rem in changelog.Removed)
            {
                if (added.ContainsKey(rem))
                {
                    added.Remove(rem);
                }
                else if (removed.Contains(rem))
                {
                }
                else if (modified.ContainsKey(rem))
                {
                    modified.Remove(rem);
                    if (renamed.ContainsKey(rem))
                    {
                        renamed.TryGetValue(rem, out string old);
                        removed.Add(old); //adds original name to removed
                    }
                }
                else if (renamed.ContainsKey(rem))
                {
                    renamed.TryGetValue(rem, out string old);
                    removed.Add(old); //adds original name to removed
                }
                else
                {
                    removed.Add(rem);
                }
            }
        }

        /// <summary>
        /// Aggregates all of changelog.Renamed
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="changelog"></param>
        private void AggregateRen(string hash, Changelog changelog)
        {
            foreach (var ren in changelog.Renamed)
            {
                if (renamed.ContainsKey(ren.Key)) //if a rename to this key has already occurred
                {
                    renamed.TryGetValue(ren.Key, out string test);
                    if (test != ren.Value) //if the previous rename does not match this rename
                    {
                        throw new FormatException("Attempted to rename to a taken name");
                    }
                }
                else
                {
                    if (renamed.ContainsKey(ren.Value))
                    {
                        //replace b->a with c->a from c->b
                        renamed.TryGetValue(ren.Value, out string old);
                        renamed.Remove(ren.Value);
                        renamed.Add(ren.Key, old);

                    }
                    else
                    {
                        renamed.Add(ren.Key, ren.Value);
                    }

                    if (added.ContainsKey(ren.Value))
                    {
                        //rename added file and remove rename
                        added.TryGetValue(ren.Value, out Tuple<string, string> old);
                        added.Remove(ren.Value);
                        added.Add(ren.Key, old);
                        renamed.Remove(ren.Key);
                    }
                    else if (modified.ContainsKey(ren.Value))
                    {
                        //rename modified file
                        modified.TryGetValue(ren.Value, out Tuple<string, string> old);
                        modified.Remove(ren.Value);
                        modified.Add(ren.Key, old);
                    }
                    else if (removed.Contains(ren.Value))
                    {
                        throw new FormatException("Cannot rename a removed file");
                    }
                }
            }
        }

        /// <summary>
        /// Converts this to a Changelog
        /// </summary>
        /// <returns></returns>
        public Changelog GetChangelog()
        {
            var added = new Dictionary<string, string>();
            foreach (var a in this.added)
            {
                added.Add(a.Key, a.Value.Item1);
            }
            var modified = new Dictionary<string, string>();
            foreach (var m in this.modified)
            {
                modified.Add(m.Key, m.Value.Item1);
            }
            var removed = new HashSet<string>();
            foreach (var r in this.removed)
            {
                removed.Add(r);
            }
            var renamed = new Dictionary<string, string>();
            foreach (var r in this.renamed)
            {
                renamed.Add(r.Key, r.Value);
            }
            return new Changelog(modified, added, removed, renamed);
        }
    }
}
