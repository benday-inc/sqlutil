using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace Benday.SqlUtils.UnitTests
{
    [TestClass]
    public class ImageFixture
    {
        [TestMethod]
        public void CreateIcon()
        {
            var img = System.Drawing.Image.FromFile(@"C:\code\repos\sql-server-utils\Benday.SqlUtils\src\Benday.SqlUtils.WpfUi\shovel-400-400.png");

            var icon = IconFromImage(img);

            var filename = @"C:\code\repos\sql-server-utils\Benday.SqlUtils\src\Benday.SqlUtils.WpfUi\shovel-400-400.ico";

            using (var stream = new FileStream(filename, FileMode.OpenOrCreate))
            {
                icon.Save(stream);
                stream.Flush();
                stream.Close();
            }
        }

        public static Icon IconFromImage(System.Drawing.Image img)
        {
            var ms = new System.IO.MemoryStream();
            var bw = new System.IO.BinaryWriter(ms);
            // Header
            bw.Write((short)0);   // 0 : reserved
            bw.Write((short)1);   // 2 : 1=ico, 2=cur
            bw.Write((short)1);   // 4 : number of images
                                  // Image directory
            var w = img.Width;
            if (w >= 256) w = 0;
            bw.Write((byte)w);    // 0 : width of image
            var h = img.Height;
            if (h >= 256) h = 0;
            bw.Write((byte)h);    // 1 : height of image
            bw.Write((byte)0);    // 2 : number of colors in palette
            bw.Write((byte)0);    // 3 : reserved
            bw.Write((short)0);   // 4 : number of color planes
            bw.Write((short)0);   // 6 : bits per pixel
            var sizeHere = ms.Position;
            bw.Write((int)0);     // 8 : image size
            var start = (int)ms.Position + 4;
            bw.Write(start);      // 12: offset of image data
                                  // Image data
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            var imageSize = (int)ms.Position - start;
            ms.Seek(sizeHere, System.IO.SeekOrigin.Begin);
            bw.Write(imageSize);
            ms.Seek(0, System.IO.SeekOrigin.Begin);

            // And load it
            return new Icon(ms);
        }




    }
}
