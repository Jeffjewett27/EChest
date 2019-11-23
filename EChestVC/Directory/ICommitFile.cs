using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using EChestVC.Model;

namespace EChestVC.Directory
{
    interface ICommitFile
    {
        Changelog GetChangelog();
        Commit GetCommit();
        string GetCommitHash();
        StreamReader GetFile(string relativePath);
        StreamReader[] GetFiles(string[] relativePaths);
    }
}
