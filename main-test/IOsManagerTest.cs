using System;
using System.IO;
using main;
using Xunit;


namespace main_test
{
    public class IOsManagerTest
    {
        public IOsManagerTest()
        {
        }

        [Fact]
        public void ShouldGetScreenShot()
        {
            IOsManager manager = new IOsManager();
            var image = manager.GetScreenShot();
            var imagePath = "test/screenshot.png";

            byte[] iBytes = Convert.FromBase64String(image);
            File.WriteAllBytes(imagePath, iBytes);

            Assert.True(File.Exists(imagePath));
        }
    }
}
