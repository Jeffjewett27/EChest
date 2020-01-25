using System;
using System.Collections.Generic;
using System.Text;

namespace EChestVC.Model
{
    public class Build : Commit
    {
        public Build(Build parent, Changelog changelog, Version version, BuildMetadata metadata) 
            : base(parent, changelog, version, metadata) { }

        public Build(Build parent, Changelog changelog, Version version, BuildMetadata metadata, string hash)
            : base(parent, changelog, version, metadata, hash) { }

        /// <summary>
        /// Aggregates a Build changelog onto a Commit AggregatedChangelog
        /// </summary>
        /// <param name="aggregated">The AggragatedChangelog from a Commit</param>
        /// <returns></returns>
        public AggregatedChangelog AggregateFrom(AggregatedChangelog aggregated)
        {
            if (IsNull)
            {
                return aggregated;
            }
            var parent = (Build)Parents[0];
            AggregatedChangelog aggregate = parent.AggregateFrom(aggregated);
            aggregate.AggregateNext(this);
            return aggregate;
        }
    }
}
