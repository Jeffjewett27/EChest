using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.Model;

namespace EChestVC.ViewModel
{
    public class InitializeCommand
    {
        private ProjectDirectory directory;

        public InitializeCommand(string path)
        {
            directory = new ProjectDirectory(path);
        }

        public void Initialize()
        {
            directory.Initialize();
        }
    }
}
