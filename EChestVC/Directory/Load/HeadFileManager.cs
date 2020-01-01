using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;
using EChestVC.Model;
using EChestVC.Directory.JSON;

namespace EChestVC.Directory.Load
{
    static class HeadFileManager
    {
        /// <summary>
        /// Loads a Head from the file directory
        /// </summary>
        /// <param name="filepath">The absolute path to the file</param>
        /// <param name="directory">The directory to load dependencies</param>
        /// <returns></returns>
        public static Head LoadHead(string filepath, DirectoryStructure directory)
        {
            string json;
            HeadJSON headJSON;
            try
            {
                json = File.ReadAllText(filepath);
            }
            catch
            {
                throw new ArgumentException("Could not read file " + filepath);
            }
            headJSON = JsonSerializer.Deserialize<HeadJSON>(json);
            return headJSON.GetHead(directory);
        } 
    }
}
