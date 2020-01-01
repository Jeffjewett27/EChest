using Microsoft.VisualStudio.TestTools.UnitTesting;
using EChestVC.Model;
using EChestVC.Directory;
using EChestVC.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using Version = EChestVC.Model.Version;

namespace EChestVC.Tests
{
    [TestClass]
    public class ModelTests
    {
        private const string path = @"C:\Users\jeffr\Documents\EChest\Big Blocks\Resource Packs\Format v3";
        private DirectoryStructure directory = new DirectoryStructure(path);
        [TestMethod]
        public void GetChangelog()
        {
            Changelog c = directory.GetChangelog("mergeOtherMain");
            Assert.IsTrue(c.Added.ContainsKey("file6.txt"));
            Assert.IsTrue(c.Modified.ContainsKey("file2.txt"));
            c.Renamed.TryGetValue("file1.txt", out string f4txt);
            Assert.AreEqual(f4txt, "file4.txt");
            Assert.IsTrue(c.Removed.Contains("file3.txt"));
        }

        [TestMethod]
        public void CreateAndDeleteChangelog()
        {
            string cpath = path + "\\Changelogs\\test.json";
            Changelog c = new Changelog("test");
            directory.CreateChangelog(c);
            Assert.IsTrue(File.Exists(cpath));
            string text = File.ReadAllText(cpath);
            Assert.AreEqual(text, "{\r\n  \"Hash\": \"test\",\r\n  \"Added\": [],\r\n  \"Modified\": [],\r\n  \"Removed\": [],\r\n  \"Renamed\": []\r\n}");
            directory.DeleteChangelog(c);
            Assert.IsFalse(File.Exists(cpath));
        }

        [TestMethod]
        public void GetVersion()
        {
            Version v = directory.GetVersion("init", false);
            string vh = v.Data.Hash;
            Assert.AreEqual(v.Hash, "init");
            VersionData f1txt = v.Data.Children["file1.txt"];
            StreamReader sr = new StreamReader(f1txt.Data);
            string result = sr.ReadToEnd();
            Assert.AreEqual(result, "file1 v1");
            sr.Dispose();
        }

        [TestMethod]
        public void CreateVersion()
        {
            Version v = directory.GetWorkingVersion();
            directory.CreateVersion(v);
        }

        [TestMethod]
        public void MakeChangelog()
        {
            Version v1 = directory.GetVersion("updateTest");
            Version v2 = directory.GetVersion("originalTest");
            Changelog changelog = v2.GetChangelog(v1);
            int count = changelog.Added.Count;
        }

        [TestMethod]
        public void GetCommit()
        {
            Commit c = directory.GetCommit("init");
            Assert.AreEqual(c.Hash, "init");
            Assert.IsTrue(c.Changelog.Added.ContainsKey("file1.txt"));
            Assert.AreEqual(c.Version.Data.Children.Count, 2);
        }

        [TestMethod]
        public void CreateAndDeleteCommit()
        {
            string cpath = path + "\\Commits\\test.json";
            Commit c = new Commit(new Commit[] { directory.GetCommit("init") }, directory.GetChangelog("initChild"), directory.GetVersion("initChild"), 
                new CommitMetadata(""), "test");
            directory.CreateCommit(c);
            Assert.IsTrue(File.Exists(cpath));
            string text = File.ReadAllText(cpath);
            Assert.AreEqual(text, "{\r\n  \"Hash\": \"test\",\r\n  \"Parents\": [\r\n    \"init\"\r\n  ],\r\n  \"Changelog\": \"initChild\",\r\n  \"Version\": \"initChild\",\r\n  \"Metadata\": {}\r\n}");
            directory.DeleteCommit(c);
            Assert.IsFalse(File.Exists(cpath));
        }

        [TestMethod]
        public void DiffWorkingVersion()
        {
            Version v1 = directory.GetVersion("updateTest");
            Version v2 = directory.GetVersion("originalTest");
            Changelog changelog = v2.GetChangelog(v1);
            Version v = v1.GetChangelogVersion(changelog);
            directory.CreateVersion(v);
        }

        [TestMethod]
        public void AggregateChanges()
        {
            Commit c1 = directory.GetCommit("init");
            Commit c2 = directory.GetCommit("mainBranch2");
            AggregatedChangelog aggregated = c2.AncestorChangelog(c1);
            string test = "";
            Version v = directory.AggregateVersion(aggregated);
            test = "";
            directory.CreateVersion(v);
        }

        [TestMethod]
        public void CommitCommand()
        {
            var command = new CommitCommand(@"C:\Users\jeffr\Documents\EChest\Big Blocks\Resource Packs\Format v3");
            command.Create("my first test");
        }
    }

    
}
