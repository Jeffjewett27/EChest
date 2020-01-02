using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EChestVC.Model
{
    /// <summary>
    /// Represents a single node in the version tree. Contains metadata, parent commits, changelog, and a hash
    /// </summary>
    public class Commit
    {
        private readonly string commitHash;
        private readonly Version version;
        private readonly Commit[] parents;
        private readonly Changelog changelog;
        private readonly CommitMetadata metadata;
        private readonly bool isNull;

        public string Hash => commitHash;
        public virtual CommitMetadata Metadata => metadata;
        public virtual Changelog Changelog => changelog;
        public virtual Commit[] Parents => parents;
        public virtual Version Version => version;
        public bool IsNull => isNull;

        public Commit(Commit[] parents, Changelog changelog, Version version, CommitMetadata metadata)
        {
            this.parents = parents;
            this.changelog = changelog;
            this.metadata = metadata;
            this.version = version;
            commitHash = GenerateHash();
        }

        public Commit(Commit[] parents, Changelog changelog, Version version, CommitMetadata metadata, string hash)
        {
            this.parents = parents;
            this.changelog = changelog;
            this.metadata = metadata;
            this.version = version;
            commitHash = hash;
        }

        public Commit(Commit parent, Changelog changelog, Version version, CommitMetadata metadata)
        {
            parents = new Commit[] { parent };
            this.changelog = changelog;
            this.metadata = metadata;
            this.version = version;
            commitHash = GenerateHash();
        }

        public Commit(Commit parent, Changelog changelog, Version version, CommitMetadata metadata, string hash)
        {
            parents = parent != null? new Commit[] { parent } : new Commit[0];
            this.changelog = changelog;
            this.metadata = metadata;
            this.version = version;
            commitHash = hash;
        }

        private Commit()
        {
            isNull = true;
        }

        public static Commit Null()
        {
            return new Commit();
        }

        /// <summary>
        /// Gets a Changelog of the differences between two commits since their least common ancestor
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual Changelog DivergentChangelog(Commit other)
        {
            Commit ancestor = LeastCommonAncestor(other);
            //Changelog thisChangelog = AncestorChangelog(ancestor);
            //Changelog otherChangelog = other.AncestorChangelog(ancestor);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Agregates a changelog from this commit to its ancestor
        /// </summary>
        /// <param name="ancestor">this and ancestor must form a boolean algrebra with ancestor at the top</param>
        /// <returns></returns>
        public AggregatedChangelog AncestorChangelog(Commit ancestor)
        {
            if (ancestor.Hash == Hash) {
                return AggregatedChangelog.EmptyChangelog();
            }
            Commit next = Parents[0];
            for (int i = 1; i < Parents.Length; i++)
            {
                next = LCAFind(next, Parents[i]);
            }
            AggregatedChangelog aggregate = next.AncestorChangelog(ancestor);
            aggregate.AggregateNext(this);
            return aggregate;
        }

        /// <summary>
        /// Aggregates the changelog from initial commit to this commit
        /// </summary>
        /// <returns></returns>
        public AggregatedChangelog AggregateChangelog()
        {
            if (IsNull)
            {
                return AggregatedChangelog.EmptyChangelog();
            }
            if (Parents.Length == 0 || Parents[0].IsNull)
            {
                return new AggregatedChangelog(this);
            }
            Commit next = Parents[0];
            for (int i = 1; i < Parents.Length; i++)
            {
                next = LCAFind(next, Parents[i]);
            }
            AggregatedChangelog aggregate = next.AggregateChangelog();
            aggregate.AggregateNext(this);
            return aggregate;
        }

        /// <summary>
        /// Finds the Commit who is the least common ancestor of this Commit and other 
        /// (who can be reached by traveling up any parent)
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Commit LeastCommonAncestor(Commit other)
        {
            if (other.commitHash == commitHash)
            {
                return this;
            }
            return LCAFind(this, other);
        }

        private static Commit LCAFind(Commit first, Commit other)
        {
            var searched = new HashSet<string>();
            searched.Add(first.commitHash);
            searched.Add(other.commitHash);
            LinkedList<Commit> branches = new LinkedList<Commit>();
            branches.AddLast(first);
            branches.AddLast(other);
            LinkedListNode<Commit> cur = branches.First;
            while (branches.Count > 1)
            {
                if (!cur.Value.isNull)
                {
                    foreach (Commit parent in cur.Value.Parents)
                    {
                        if (!searched.Contains(parent.commitHash) && !parent.isNull)
                        {
                            branches.AddBefore(cur, parent);
                            searched.Add(parent.commitHash);
                        }
                    }
                }
                LinkedListNode<Commit> temp = cur;
                cur = cur.Next != null ? cur.Next : branches.First;
                branches.Remove(temp);
            }
            if (branches.Count == 0)
            {
                throw new FormatException("Commit hierarchy not configured properly");
            }
            if (branches.First.Value.isNull)
            {
                throw new FormatException("Commits do not contain a common ancestor");
            }
            return branches.First.Value;
        }

        /// <summary>
        /// Gets an array of this Commit's Parents' hashes
        /// </summary>
        /// <returns></returns>
        private string[] GetParentHashes()
        {
            return (from p in Parents select p.Hash).ToArray();
        }

        /// <summary>
        /// Gets the hash of this Commit's Version
        /// </summary>
        /// <returns></returns>
        private string GetVersionHash()
        {
            return Version.Hash;
        }

        /// <summary>
        /// Gets the hash of this Commit's Changelog
        /// </summary>
        /// <returns></returns>
        private string GetChangelogHash()
        {
            return Changelog.Hash;
        }

        /// <summary>
        /// Generates this Commit's hash
        /// </summary>
        /// <returns></returns>
        private string GenerateHash()
        {
            StringBuilder sum = new StringBuilder();
            sum.Append(GetVersionHash());
            sum.Append(GetChangelogHash());
            foreach (string s in GetParentHashes())
            {
                sum.Append(s);
            }
            sum.Append(Metadata.GetHash());
            return Model.Hash.ComputeHash(sum.ToString());
        }
    }
}
