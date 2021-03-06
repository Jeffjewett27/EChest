﻿using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.Model;

namespace EChestVC.Model
{
    /// <summary>
    /// Contains the Metadata of a Commit, beyond its hash references
    /// </summary>
    public class CommitMetadata : Metadata
    {
        private string hash;
        //TODO: add data fields
        public override string Hash => hash;

        public CommitMetadata(DateTime createdTime, string[] authors, string message) 
            : base(createdTime, authors, message)
        {
            hash = GenerateHash();
        }

        protected override string GenerateHash()
        {
            return base.GenerateHash();
        }
    }
}
