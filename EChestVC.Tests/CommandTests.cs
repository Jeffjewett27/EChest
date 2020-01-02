using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.ViewModel;

namespace EChestVC.Tests
{
    [TestClass]
    public class CommandTests
    {
        [TestMethod]
        public void CommitCommand()
        {
            var command = new CommitCommand(@"C:\Users\jeffr\Documents\EChest\Big Blocks\Resource Packs\FormatTest");
            command.Create("my first test");
        }

        [TestMethod]
        public void InitializeCommand()
        {
            var command = new InitializeCommand(@"C:\Users\jeffr\Documents\EChest\Big Blocks\Resource Packs\FormatTest");
            command.Initialize();
        }
    }
}
