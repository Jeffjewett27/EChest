using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EChestVC.Model
{
    /// <summary>
    /// Represents a file or a directory in a tree structure
    /// </summary>
    public class VersionData
    {
        public enum FileType
        {
            File,
            Directory
        }

        private readonly FileType filetype;
        private readonly Stream data;
        private readonly VDKeyedCollection children;
        private readonly string filename;
        private readonly string hash;

        public virtual Stream Data {
            get {
                data.Seek(0, SeekOrigin.Begin);
                return data;
            }
        }
        public virtual VDKeyedCollection Children => children;
        public string Filename => filename;
        public virtual string Hash => hash;
        public FileType Filetype => filetype;
        
        /// <summary>Constructs a VersionData of type FileType.File</summary>
        /// <param name="filename">Just the filename, with no preceding directories</param>
        /// <param name="data">The stream of this file's content</param>
        /// <param name="hash">This VersionData's hash</param>
        public VersionData(string filename, Stream data, string hash)
        {
            this.data = data;
            this.hash = hash;
            this.filename = filename;
            this.filetype = FileType.File;
        }

        /// <summary>Constructs a VersionData of type FileType.File</summary>
        /// <param name="filename">Just the filename, with no preceding directories</param>
        /// <param name="data">The stream of this file's content</param>
        public VersionData(string filename, Stream data)
        {
            this.data = data;
            this.filename = filename;
            this.filetype = FileType.File;
            this.hash = GetHash();
        }

        /// <summary>Constructs a VersionData of type FileType.Directory</summary>
        /// <param name="filename">Just the filename, with no preceding directories</param>
        /// <param name="children">The VersionDatas contained in this directory</param>
        /// <param name="hash">This VersionData's hash</param>
        public VersionData(string filename, VDKeyedCollection children, string hash)
        {
            this.filename = filename;
            this.children = children;
            this.hash = hash;
        }

        /// <summary>Constructs a VersionData of type FileType.Directory</summary>
        /// <param name="filename">Just the filename, with no preceding directories</param>
        /// <param name="children">The VersionDatas contained in this directory</param>
        public VersionData(string filename, VDKeyedCollection children)
        {
            this.filename = filename;
            this.children = children;
            this.filetype = FileType.Directory;
            this.hash = GetHash();
        }

        /// <summary>
        /// Generates this VersionData's hash
        /// </summary>
        /// <returns></returns>
        private string GetHash()
        {
            if (filetype == FileType.Directory)
            {
                return children.GetHash();
            } else
            {
                return Model.Hash.ComputeHash(data);
            }
        }

        /// <summary>
        /// Searches recursively for a VersionData of a given filepath
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public VersionData PathGetFile(string path)
        {
            //path example: thisfilename/childfilename/morestuff
            string rootDir = VersionDataPath.GetRootDirectory(path);
            if (rootDir == path)
            {
                if (filename == path)
                {
                    return this;
                } else
                {
                    throw new ArgumentException("file or directory " + path + " not found");
                }
            }
            if (filetype == FileType.File)
            {
                throw new ArgumentException(filename + " is not a directory for " + path);
            }
            string childPath = VersionDataPath.RemoveRootDirectory(path); //childPath example: childfilename/morestuff
            string childName = VersionDataPath.GetRootDirectory(childPath); //childName example: childfilename
            if (children.TryGetValue(childName, out VersionData child))
            {
                return child.PathGetFile(childPath);
            } else
            {
                throw new ArgumentException("");
            }
        }

        /// <summary>
        /// Recursively builds a Changelog by comparing each directory to its analogue and finding differences
        /// </summary>
        /// <param name="original"></param>
        /// <param name="update"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static ChangelogBuilder BuildChangelog(VersionData original, VersionData update, string prefix = "")
        {
            ChangelogBuilder changelog = new ChangelogBuilder();
            var byHash = new Dictionary<string, LinkedList<string>>();
            var otherByHash = new Dictionary<string, LinkedList<string>>();
            var byName = new VDKeyedCollection();
            var otherByName = new VDKeyedCollection();
            if (original == null)
            {
                if (update == null)
                {
                    throw new ArgumentNullException("original and update");
                }
            } else if (original.filetype == FileType.File)
            {
                throw new InvalidOperationException("Cannot call GetChangelog on a FileType.File type");
            }
            if (update != null && update.filetype == FileType.File)
            {
                throw new ArgumentException("child must be of type FileType.File");
            }
            if (update == null)
            {
                AggregateRemove(changelog, prefix, original);
                return changelog;
            }
            if (original == null)
            {
                AggregateAdd(changelog, prefix, update);
                return changelog;
            }
            AddAlteredFiles(original.children, update.children, byName, byHash);
            AddAlteredFiles(update.children, original.children, otherByName, otherByHash);

            FindRenames(changelog, prefix, byName, otherByName, byHash, otherByHash);
            FindModifieds(changelog, prefix, byName, otherByName);
            FindAddRemoveds(changelog, prefix, byName, otherByName);

            return changelog;
        }

        /// <summary>
        /// Selects all files that were altered in some way (added, modified, removed, or renamed)
        /// </summary>
        /// <param name="thisVD"></param>
        /// <param name="otherVD"></param>
        /// <param name="byName"></param>
        /// <param name="byHash"></param>
        private static void AddAlteredFiles(VDKeyedCollection thisVD, VDKeyedCollection otherVD, VDKeyedCollection
                byName, Dictionary<string, LinkedList<string>> byHash)
        {
            foreach (var pair in thisVD)
            {
                string name = pair.Filename;
                string hash = pair.Hash;
                bool contains = false;
                if (otherVD.Contains(name)) //if the file has not been altered
                {
                    contains = true;
                    if (hash == otherVD[name].Hash)
                    {
                        continue;
                    }
                }
                if (!byHash.ContainsKey(hash))
                {
                    byHash[hash] = new LinkedList<string>();
                }
                if (contains) //prioritize renaming files with no counterpart
                {
                    byHash[hash].AddLast(name);
                }
                else
                {
                    byHash[hash].AddFirst(name);
                }
                byName.Add(pair);
            }
        }

        /// <summary>
        /// Finds objects of the same hash but different names
        /// </summary>
        private static void FindRenames(ChangelogBuilder changelog, string prefix, VDKeyedCollection byName, 
            VDKeyedCollection otherByName, Dictionary<string, LinkedList<string>> byHash, 
            Dictionary<string, LinkedList<string>> otherByHash)
        {
            foreach (var pair in byHash)
            {
                if (otherByHash.ContainsKey(pair.Key))
                {
                    var thisll = pair.Value;
                    var otherll = otherByHash[pair.Key];
                    while (thisll.Count > 0 && otherll.Count > 0)
                    {
                        string thisfile = thisll.First.Value;
                        string otherfile = otherll.First.Value;
                        byName.Remove(thisfile);
                        otherByName.Remove(otherfile);
                        string thisfpath = PrefixFilename(prefix, thisfile);
                        string otherfpath = PrefixFilename(prefix, otherfile);
                        changelog.Rename(thisfpath, otherfpath);
                        thisll.RemoveFirst();
                        otherll.RemoveFirst();
                    }
                }
            }
        }

        /// <summary>
        /// Finds objects of the same name, but different hashes
        /// </summary>
        private static void FindModifieds(ChangelogBuilder changelog, string prefix, VDKeyedCollection byName, 
            VDKeyedCollection otherByName)
        {
            var toRemove = new LinkedList<string>();
            foreach (var thisVD in otherByName)
            {
                //if (byName.ContainsKey(pair.Key))
                if (byName.Contains(thisVD.Filename))
                {
                    VersionData otherVD = byName[thisVD.Filename];
                    if (thisVD.Filetype == otherVD.Filetype)
                    {
                        string fpath = PrefixFilename(prefix, thisVD.Filename);
                        changelog.Modify(fpath, thisVD.Hash);
                        if (thisVD.Filetype == FileType.Directory)
                        {
                            var subchangelog = BuildChangelog(thisVD, otherVD, fpath);
                            changelog.Aggregate(subchangelog);
                        }
                        toRemove.AddLast(thisVD.Filename);
                    }
                }
            }
            foreach (string s in toRemove)
            {
                byName.Remove(s);
                otherByName.Remove(s);
            }
        }

        /// <summary>
        /// Finds objects that exist in one collection but not the other
        /// </summary>
        private static void FindAddRemoveds(ChangelogBuilder changelog, string prefix, VDKeyedCollection byName, 
            VDKeyedCollection otherByName)
        {
            foreach (var pair in byName)
            {
                string fpath = PrefixFilename(prefix, pair.Filename);
                changelog.Remove(fpath);
                if (pair.Filetype == FileType.Directory)
                {
                    var subchangelog = BuildChangelog(pair, null, fpath);
                    changelog.Aggregate(subchangelog);
                }
            }
            foreach (var pair in otherByName)
            {
                string fpath = PrefixFilename(prefix, pair.Filename);
                changelog.Add(fpath, pair.Hash);
                if (pair.Filetype == FileType.Directory)
                {
                    var subchangelog = BuildChangelog(null, pair, fpath);
                    changelog.Aggregate(subchangelog);
                }
            }
        }

        private static string PrefixFilename(string prefix, string filename)
        {
            return VersionDataPath.PrefixFilename(prefix, filename);
        }

        /// <summary>
        /// Removes all descendant files of a Directory to a changelog
        /// </summary>
        private static void AggregateRemove(ChangelogBuilder changelog, string prefix, VersionData original)
        {
            foreach (var child in original.Children)
            {
                string fpath = PrefixFilename(prefix, child.Filename);
                changelog.Remove(fpath);
                if (child.Filetype == FileType.Directory)
                {
                    var subchangelog = BuildChangelog(child, null, fpath);
                    changelog.Aggregate(subchangelog);
                }
            }
        }

        /// <summary>
        /// Adds all descendant files of a Directory to a changelog
        /// </summary>
        private static void AggregateAdd(ChangelogBuilder changelog, string prefix, VersionData update)
        {
            foreach (var child in update.Children)
            {
                string fpath = PrefixFilename(prefix, child.Filename);
                changelog.Add(fpath, child.Hash);
                if (child.Filetype == FileType.Directory)
                {
                    var subchangelog = BuildChangelog(child, null, fpath);
                    changelog.Aggregate(subchangelog);
                }
            }
        }

        /// <summary>
        /// Returns a VersionData that contains only those children contained in a changelog
        /// </summary>
        /// <param name="changelog"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public VersionData Trim(Changelog changelog, string prefix = "")
        {
            if (Filetype != FileType.Directory)
            {
                throw new InvalidOperationException("Trim cannot operate on a FileType.File");
            }
            var vdcollection = new VDKeyedCollection();
            foreach (var vd in children)
            {
                string fpath = PrefixFilename(prefix, vd.Filename);
                if (changelog.Added.ContainsKey(fpath) || changelog.Modified.ContainsKey(fpath))
                {
                    if (vd.Filetype == FileType.Directory)
                    {
                        vdcollection.Add(vd.Trim(changelog, fpath));
                    }
                    else
                    {
                        vdcollection.Add(new VersionData(vd.Filename, vd.Data));
                    }
                }
            }
            return new VersionData(Filename, vdcollection);
        }
    }
}
