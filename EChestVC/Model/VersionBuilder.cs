using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.HelperFunctions;

namespace EChestVC.Model
{
    /// <summary>
    /// Builds up a Version (which is an immutable type)
    /// </summary>
    public class VersionBuilder
    {
        /// <summary>
        /// A mutable wrapper for a VersionData tree
        /// </summary>
        private class DataNode
        {
            public string filename;
            public VersionData data;
            public GenericKeyedCollection<string, DataNode> children;

            public DataNode(string filename)
            {
                children = new GenericKeyedCollection<string, DataNode>(node => node.filename);
            }

            public DataNode(VersionData data)
            {
                children = new GenericKeyedCollection<string, DataNode>(node => node.filename);
                this.data = data;
                filename = data.Filename;
            }
        }

        private DataNode root;

        public VersionBuilder()
        {
            root = new DataNode("root");
        }

        public VersionBuilder(Version version)
        {
            root = new DataNode("root");
            AddDataDirectory(version.Data, root);
        }

        /// <summary>
        /// Adds all the descendants of vdata to root
        /// </summary>
        /// <param name="vdata"></param>
        /// <param name="node"></param>
        private void AddDataDirectory(VersionData vdata, DataNode node)
        {
            if (vdata.Filetype != VersionData.FileType.Directory)
            {
                throw new ArgumentException("vdata must be of type Directory");
            }
            foreach (var child in vdata.Children)
            {
                DataNode cnode = new DataNode(child);
                node.children.Add(cnode);
                if (child.Filetype == VersionData.FileType.Directory)
                {
                    AddDataDirectory(child, cnode);
                }
            }
        }

        /// <summary>
        /// Adds a VersionData to the tree in its specified path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="vdata"></param>
        public void AddVersionData(string path, VersionData vdata)
        {
            string[] directoryPath = VersionDataPath.SplitDirectories(path);
            DataNode node = GetParentNode(directoryPath);
            string filename = directoryPath[directoryPath.Length - 1];
            if (node.children.Contains(filename))
            {
                node.children[filename].data = vdata;
            } else
            {
                node.children.Add(new DataNode(vdata));
            }
        }

        public void RenameVersionData(string oldPath, string newPath)
        {
            string[] oldDirPath = VersionDataPath.SplitDirectories(oldPath);
            string[] newDirPath = VersionDataPath.SplitDirectories(newPath);
            DataNode parentNode = GetParentNode(oldDirPath);
            string oldfilename = oldDirPath[oldDirPath.Length - 1];
            string newfilename = newDirPath[newDirPath.Length - 1];
            if (!parentNode.children.Contains(oldfilename))
            {
                throw new ArgumentException(oldPath + " does not exist");
            }
            DataNode node = parentNode.children[oldfilename];
            node.filename = newfilename;
            parentNode.children.Remove(node);
            parentNode = GetParentNode(newDirPath);
            parentNode.children.Add(node);
        }

        /// <summary>
        /// Converts this to a Version
        /// </summary>
        /// <returns></returns>
        public Version GetVersion()
        {
            var vdata = GetDirectoryData(root);
            return new Version(vdata);
        }

        private DataNode GetParentNode(string[] directoryPath)
        {
            DataNode node = root;
            for (int i = 0; i < directoryPath.Length - 1; i++)
            {
                string dir = directoryPath[i];
                if (!node.children.Contains(dir))
                {
                    node.children.Add(new DataNode(dir));
                }
                node = node.children[dir];
            }
            return node;
        }

        /// <summary>
        /// Converts a DataNode and all descendants to a VersionData
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private VersionData GetDirectoryData(DataNode node)
        {
            VDKeyedCollection children = new VDKeyedCollection();
            foreach (var childNode in node.children)
            {
                if (childNode.data == null)
                {
                    continue;
                }
                if (childNode.data.Filetype == VersionData.FileType.Directory)
                {
                    children.Add(GetDirectoryData(childNode));
                } else
                {
                    var vd = childNode.data;
                    children.Add(new VersionData(childNode.filename, vd.Data, vd.Hash));
                }
            }
            return new VersionData(node.filename, children);
        }
    }
}
