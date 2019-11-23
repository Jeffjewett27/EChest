using System;
using System.Collections.Generic;
using System.Text;

namespace EChestVC.Model
{
    /// <summary>
    /// Represents an aggregation of changelogs down a branch of commits. Stores information about which commits files are located in.
    /// </summary>
    class AggregatedChangelog
    {
        private Dictionary<string, string> modified; //file->hash
        private Dictionary<string, string> added; //file->hash
        private HashSet<string> removed; //file
        private Dictionary<string, string> renamed; //new file-> old file

        public AggregatedChangelog(string hash, Changelog changelog)
        {
            modified = new Dictionary<string, string>();
            added = new Dictionary<string, string>();
            removed = new HashSet<string>();
            renamed = new Dictionary<string, string>();

            AggregateNext(hash, changelog);
        }

        private AggregatedChangelog()
        {
            modified = new Dictionary<string, string>();
            added = new Dictionary<string, string>();
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
                if (added.ContainsKey(add))
                {
                    added[add] = hash;
                }
                else if (removed.Contains(add))
                {
                    modified.Add(add, hash);
                    removed.Remove(add);
                }
                else if (modified.ContainsKey(add))
                {
                    throw new FormatException("Attempted to add an existing and modified file");
                }
                else
                {
                    added.Add(add, hash);
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
                if (added.ContainsKey(mod))
                {
                    added[mod] = hash;
                }
                else if (removed.Contains(mod))
                {
                    throw new FormatException("Attempted to modify a removed file");
                }
                else if (modified.ContainsKey(mod))
                {
                    added[mod] = hash;
                }
                else
                {
                    modified.Add(mod, hash);
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
                        added.TryGetValue(ren.Value, out string old);
                        added.Remove(ren.Value);
                        added.Add(ren.Key, old);
                        renamed.Remove(ren.Key);
                    }
                    else if (modified.ContainsKey(ren.Value))
                    {
                        //rename modified file
                        modified.TryGetValue(ren.Value, out string old);
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
    }
}
