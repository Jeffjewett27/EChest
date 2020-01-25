using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EChestVC.Model
{
    /// <summary>
    /// Represents an object's Metadata. Hash must be immutable; mutable properties cannot affect Hash
    /// </summary>
    public abstract class Metadata
    {
        private DateTime createdTime;
        private string[] authors;
        private string message;

        public DateTime CreatedTime => createdTime;
        public string[] Authors => authors;
        public string Message => message;
        public abstract string Hash { get; }

        public Metadata(DateTime createdTime, string[] authors, string message)
        {
            this.createdTime = createdTime;
            this.authors = authors;
            this.message = message;
        }

        protected virtual string GenerateHash()
        {
            var builder = new StringBuilder();
            builder.Append(createdTime.ToBinary());
            Array.ForEach(authors, a =>
            {
                builder.Append(a);
                builder.Append(",");
            });
            builder.Append(message);
            return builder.ToString();
        }
    }
}
