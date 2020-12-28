using System;
using System.IO;

namespace main
{
    public class IOsManager
    {
        public IOsManager()
        {
        }

        public string GetScreenShot()
        {
            try
            {
                var screenshot = Pranas.ScreenshotCapture.TakeScreenshot();
                using (MemoryStream ms = new MemoryStream())
                {
                    screenshot.Save(ms, screenshot.RawFormat);
                    byte[] imageBytes = ms.ToArray();
                    return Convert.ToBase64String(imageBytes);
                }
            } catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
