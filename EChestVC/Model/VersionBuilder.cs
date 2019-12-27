﻿using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.HelperFunctions;

namespace EChestVC.Model
{
    public class VersionBuilder
    {
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

        public void AddVersionData(string path, VersionData vdata)
        {
            string[] directoryPath = VersionDataPath.SplitDirectories(path);
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
            string filename = directoryPath[directoryPath.Length - 1];
            if (node.children.Contains(filename))
            {
                node.children[filename].data = vdata;
            } else
            {
                node.children.Add(new DataNode(vdata));
            }
        }

        public Version GetVersion()
        {
            var vdata = GetDirectoryData(root);
            return new Version(vdata);
        }

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
                    children.Add(new VersionData(vd.Filename, vd.Data, vd.Hash));
                }
            }
            return new VersionData(node.filename, children);
        }
    }
}
