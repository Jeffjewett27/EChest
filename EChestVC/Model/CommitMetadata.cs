using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.Model;

namespace EChestVC.Model
{
    /// <summary>
    /// Contains the Metadata of a Commit, beyond its hash references
    /// </summary>
    public class CommitMetadata
    {
        private readonly string message;

        public string Message => message;
        //TODO: add data fields

        public CommitMetadata(string message)
        {
            this.message = message;
        }

        public string GetHash()
        {
            return Hash.ComputeHash(message);
        }
    }
}
