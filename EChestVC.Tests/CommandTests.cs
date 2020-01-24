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
            command.Create("checked out test_branch");
        }

        [TestMethod]
        public void InitializeCommand()
        {
            var command = new InitializeCommand(@"C:\Users\jeffr\Documents\EChest\Big Blocks\Resource Packs\FormatTest");
            command.Initialize();
        }

        [TestMethod]
        public void CheckoutCommand()
        {
            var command = new CheckoutCommand(@"C:\Users\jeffr\Documents\EChest\Big Blocks\Resource Packs\FormatTest");
            command.Execute(ViewModel.CheckoutCommand.Type.Branch, "test_branch");
        }

        [TestMethod]
        public void CreateBranchCommand()
        {
            var command = new BranchCommand(@"C:\Users\jeffr\Documents\EChest\Big Blocks\Resource Packs\FormatTest");
            command.Create("test_branch2");
        }
    }
}
