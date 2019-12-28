using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace EChestVC.Directory.JSON
{
    /// <summary>
    /// Sets the serialization options for JSON in an EChestVC directory
    /// </summary>
    static class JSONFileFormat
    {
        public static JsonSerializerOptions GetJsonSerializerOptions()
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            return options;
        }
    }
}
