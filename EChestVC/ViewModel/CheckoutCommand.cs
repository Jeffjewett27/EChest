using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.Model;

namespace EChestVC.ViewModel
{
    public class CheckoutCommand
    {
        public enum Type
        {
            Branch,
            Commit
        }

        private ProjectDirectory directory;

        public CheckoutCommand(string path)
        {
            directory = new ProjectDirectory(path);
        }

        public void Execute(Type type, string id)
        {
            if (type == Type.Branch)
            {
                Branch branch = directory.LoadBranch(id);
                directory.ChangeHead(branch);
            } else
            {
                Commit commit = directory.GetCommit(id);
                directory.ChangeHead(commit);
            }
        }

        public void Execute(string id)
        {
            Type type = id.Length == Hash.NumHexChars || IsHex(id)
                ? Type.Commit : Type.Branch;
            Execute(type, id);
        }

        //stack overflow: Jeremy Ruten
        private bool IsHex(IEnumerable<char> chars)
        {
            bool isHex;
            foreach (var c in chars)
            {
                isHex = ((c >= '0' && c <= '9') ||
                         (c >= 'a' && c <= 'f') ||
                         (c >= 'A' && c <= 'F'));

                if (!isHex)
                    return false;
            }
            return true;
        }
    }
}
