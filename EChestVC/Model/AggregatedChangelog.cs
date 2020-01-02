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
        private Dictionary<string, Tuple<string, Commit>> modified; //file-><datahash, versionhash>
        private Dictionary<string, Tuple<string, Commit>> added; //file->hash
        private HashSet<string> removed; //file
        private Dictionary<string, string> renamed; //new file-> old file

        public Dictionary<string, Tuple<string, Commit>> Modified => modified;
        public Dictionary<string, Tuple<string, Commit>> Added => added;
        public HashSet<string> Removed => removed;
        public Dictionary<string, string> Renamed => renamed; 

        public AggregatedChangelog(Commit commit)
        {
            modified = new Dictionary<string, Tuple<string, Commit>>();
            added = new Dictionary<string, Tuple<string, Commit>>();
            removed = new HashSet<string>();
            renamed = new Dictionary<string, string>();

            AggregateNext(commit);
        }

        private AggregatedChangelog()
        {
            modified = new Dictionary<string, Tuple<string, Commit>>();
            added = new Dictionary<string, Tuple<string, Commit>>();
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
        /// <param name="commit"></param>
        public void AggregateNext(Commit commit)
        {
            AggregateAdd(commit);

            AggregateMod(commit);

            AggregateRem(commit);

            AggregateRen(commit);
        }

        /// <summary>
        /// Aggregates all of changelog.Added
        /// </summary>
        /// <param name="commit"></param>
        private void AggregateAdd(Commit commit)
        {
            foreach (var add in commit.Changelog.Added)
            {
                if (added.ContainsKey(add.Key))
                {
                    added[add.Key] = Tuple.Create(add.Value, commit);
                }
                else if (removed.Contains(add.Key))
                {
                    modified.Add(add.Key, Tuple.Create(add.Value, commit));
                    removed.Remove(add.Key);
                }
                else if (modified.ContainsKey(add.Key))
                {
                    throw new FormatException("Attempted to add an existing and modified file");
                }
                else
                {
                    added.Add(add.Key, Tuple.Create(add.Value, commit));
                }
            }
        }

        /// <summary>
        /// Aggregates all of changelog.Modified
        /// </summary>
        /// <param name="commit"></param>
        private void AggregateMod(Commit commit)
        {
            foreach (var mod in commit.Changelog.Modified)
            {
                if (added.ContainsKey(mod.Key))
                {
                    added[mod.Key] = Tuple.Create(mod.Value, commit);
                }
                else if (removed.Contains(mod.Key))
                {
                    throw new FormatException("Attempted to modify a removed file");
                }
                else if (modified.ContainsKey(mod.Key))
                {
                    modified[mod.Key] = Tuple.Create(mod.Value, commit);
                }
                else
                {
                    modified.Add(mod.Key, Tuple.Create(mod.Value, commit));
                }
            }
        }

        /// <summary>
        /// Aggregates all of changelog.Removed
        /// </summary>
        /// <param name="commit"></param>
        private void AggregateRem(Commit commit)
        {
            foreach (var rem in commit.Changelog.Removed)
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
        /// <param name="commit"></param>
        private void AggregateRen(Commit commit)
        {
            foreach (var ren in commit.Changelog.Renamed)
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
                        added.TryGetValue(ren.Value, out Tuple<string, Commit> old);
                        added.Remove(ren.Value);
                        added.Add(ren.Key, old);
                        renamed.Remove(ren.Key);
                    }
                    else if (modified.ContainsKey(ren.Value))
                    {
                        //rename modified file
                        modified.TryGetValue(ren.Value, out Tuple<string, Commit> old);
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
