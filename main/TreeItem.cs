using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace main
{
    public class TreeItem
    {
        public string title { get; set; }
        public bool isFolder { get; set; }
        public string key { get; set; }
        public List<TreeItem> children;

        public TreeItem(FileSystemInfo fsi)
        {
            title = fsi.Name;
            children = new List<TreeItem>();

            if (fsi.Attributes == FileAttributes.Directory)
            {
                isFolder = true;
                foreach (FileSystemInfo f in (fsi as DirectoryInfo).GetFileSystemInfos())
                {
                    children.Add(new TreeItem(f));
                }
            }
            else
            {
                isFolder = false;
            }
            key = title.Replace(" ", "").ToLower();
        }

        public string GetJson()
        {
            return System.Text.Json.JsonSerializer.Serialize(this.children);
        }

    }
}
