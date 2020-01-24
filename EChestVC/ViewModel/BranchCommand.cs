using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.Model;

namespace EChestVC.ViewModel
{
    public class BranchCommand
    {
        private ProjectDirectory directory;

        public BranchCommand(string path)
        {
            directory = new ProjectDirectory(path);
        }

        public void Create(string name)
        {
            Head head = directory.GetHead();
            Commit target = head.GetTarget();
            BranchMetadata metadata = new BranchMetadata();
            Branch branch = new Branch(target, metadata, name);
            directory.CreateBranch(branch);
        }

        public void Update(string name)
        {
            Head head = directory.GetHead();
            Commit target = head.GetTarget();
            BranchMetadata metadata = new BranchMetadata();
            Branch branch = new Branch(target, metadata, name);
            directory.CreateBranch(branch);
        }

        public void Delete(string name)
        {
            Branch branch = directory.LoadBranch(name);
            directory.DeleteBranch(branch);
        }
    }
}
