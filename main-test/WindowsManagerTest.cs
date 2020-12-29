using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using main;
using System.IO;

namespace main_test
{
    public class WindowsManagerTest
    {
        [Fact]
        public void ShouldGetAllLocalDrives()
        {
            WindowsManager wmanager = new WindowsManager();
            var deviceLetters = wmanager.GetLocalDisks();

            Assert.True(deviceLetters.Count > 0);
        }

        [Fact]
        public void ShouldGetScreenshot()
        {
            WindowsManager wmanager = new WindowsManager();
            string b64 = wmanager.GetScreenShot();

            Assert.NotNull(b64);
        }

        [Fact]
        public void ShouldGetDirectoryInfo()
        {
            var json = Finder.GetDirectoryTreeInfo("test_files");
            Assert.NotNull(json);
        }
    }
}
