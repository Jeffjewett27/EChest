using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using EChestVC.Model;

namespace EChestVC.Directory.JSON
{
    /// <summary>
    /// Object to be serialized and deserialized between JSON and CommitMetadata
    /// </summary>
    class CommitMetadataJSON : MetadataJSON
    {
        public CommitMetadataJSON(CommitMetadata metadata) : base(metadata)
        {
        }

        /// <summary>
        /// Converts this object into a CommitMetadata object
        /// </summary>
        /// <returns></returns>
        public CommitMetadata GetMetadata()
        {
            var dateCreated = DateTimeString.FromString(Created);
            return new CommitMetadata(dateCreated, Authors, Message);
        }
    }
}
