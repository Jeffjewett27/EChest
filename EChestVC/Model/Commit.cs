using System;
using System.Collections.Generic;
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
            GenerateHash();
        }

        public Commit(Commit[] parents, Changelog changelog, Version version, CommitMetadata metadata, string hash)
        {
            this.parents = parents;
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
            aggregate.AggregateNext(commitHash, Changelog);
            return aggregate;
        }

        /// <summary>
        /// Aggregates the changelog from initial commit to this commit
        /// </summary>
        /// <returns></returns>
        public AggregatedChangelog AggregateChangelog()
        {
            if (Parents[0].IsNull)
            {
                return new AggregatedChangelog(Hash, Changelog);
            }
            Commit next = Parents[0];
            for (int i = 1; i < Parents.Length; i++)
            {
                next = LCAFind(next, Parents[i]);
            }
            AggregatedChangelog aggregate = next.AggregateChangelog();
            aggregate.AggregateNext(commitHash, Changelog);
            return aggregate;
        }

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

        private string[] GetParentHashes()
        {
            return null;
        }

        private string GetVersionHash()
        {
            return null;
        }

        private string GetChangelogHash()
        {
            return null;
        }

        private string GenerateHash()
        {
            if (commitHash != null)
            {
                return commitHash;
            }
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
