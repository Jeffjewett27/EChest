using System;
using System.Collections.Generic;
using System.Text;
using EChestVC.Model;

namespace EChestVC.Directory.JSON
{
    /// <summary>
    /// Object to be serialized and deserialized between JSON and CommitMetadata
    /// </summary>
    class CommitMetadataJSON
    {
        public string Message { get; set; }

        public CommitMetadataJSON(CommitMetadata metadata)
        {
            Message = metadata.Message;
        }

        public CommitMetadataJSON() { }

        /// <summary>
        /// Converts this object into a CommitMetadata object
        /// </summary>
        /// <returns></returns>
        public CommitMetadata GetMetadata()
        {
            return new CommitMetadata(Message);
        }
    }
}
