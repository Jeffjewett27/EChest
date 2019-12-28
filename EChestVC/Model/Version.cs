using System;
using System.Collections.Generic;
using System.Text;

namespace EChestVC.Model
{
    /// <summary>
    /// A wrapper for a VersionData tree
    /// </summary>
    public class Version
    {
        //data is the root of the tree
        private VersionData data;
        private string hash;

        public string Hash => hash;
        public virtual VersionData Data => data;

        public Version(VersionData data, string hash)
        {
            this.data = data;
            this.hash = hash;
        }

        public Version(VersionData data)
        {
            this.data = data;
            this.hash = GetHash();
        }

        /// <summary>
        /// Returns this Version's hash
        /// </summary>
        /// <returns></returns>
        private string GetHash()
        {
            return data.Hash;
        }

        /// <summary>
        /// Computes the Changelog between this and its descendant Version, such that "this + changelog => child"
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public Changelog GetChangelog(Version child)
        {
            return VersionData.BuildChangelog(Data, child.Data).GetChangelog();
        }

        /// <summary>
        /// Returns a subset of this Version that contains only VersionData contained in changelog
        /// </summary>
        /// <param name="changelog"></param>
        /// <returns></returns>
        public Version GetChangelogVersion(Changelog changelog)
        {
            var vd = data.Trim(changelog);
            return new Version(vd, vd.Hash);
        }

        /// <summary>
        /// Queries a VersionData using the specified filepath
        /// </summary>
        /// <param name="filepath">A relative filepath beginning at Data.Children 
        /// (eg. a file located at ".../Versions/vname/dir1/file.txt" would be queried by "dir1/file.txt"</param>
        /// <returns></returns>
        public VersionData GetVersionData(string filepath)
        {
            string path = VersionDataPath.PrefixFilename(Data.Filename, filepath);
            return Data.PathGetFile(path);
        }
    }
}
