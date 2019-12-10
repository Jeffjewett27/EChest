using Microsoft.VisualStudio.TestTools.UnitTesting;
using EChestVC.Model;
using EChestVC.Directory;
using System;
using Version = EChestVC.Model.Version;

namespace EChestVC.Tests
{
    [TestClass]
    public class ModelTests
    {
        private DirectoryStructure directory = new DirectoryStructure(@"C:\Users\jeffr\Documents\EChest\Big Blocks\Resource Packs\Format v3");
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
        public void GetVersion()
        {
            Version v = directory.GetVersion("init", false);
            string vh = v.Data.Hash;
            Assert.AreEqual(v.Hash, "init");
            v.Data.Children.TryGetValue("file1.txt", out VersionData f1txt);
            Assert.AreEqual(f1txt.Data.ReadToEnd(), "file1 v1");
        }

        [TestMethod]
        public void GetCommit()
        {
            Commit c = directory.GetCommit("init");
            Assert.AreEqual(c.Hash, "init");
            Assert.IsTrue(c.Changelog.Added.ContainsKey("file1.txt"));
            Assert.AreEqual(c.Version.Data.Children.Count, 2);
        }
    }

    
}
