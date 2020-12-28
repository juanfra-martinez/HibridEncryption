using System;
using System.Collections.Generic;
using System.IO;

namespace main
{
    public class Finder
    {
        public static IEnumerable<string> GetAllFilesFromDirectory(string path)
        {
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException($"Directory not exist: {path} ");

            return Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories);
        }

        public static bool ExcludeFile(string file)
        {
            string parent = Directory.GetParent(file).FullName;
            if (parent.Contains("Windows"))
            {
                return true;
            }

            if (Path.GetExtension(file) == ".dll")
            {
                return true;
            }

            if (Path.GetFileName(file).StartsWith("."))
            {
                return true;
            }

            return false;
        }
    }
}
