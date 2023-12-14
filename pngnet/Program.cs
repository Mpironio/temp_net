using System.Drawing;
using System.Runtime.Intrinsics;

namespace pngnet
{
    class Program
    {
        static void Main(string[] args)
        {
            string debugDir = Directory.GetCurrentDirectory();
            string projDir = Directory.GetParent(debugDir).Parent.FullName;
            string imagesDir = Directory.GetParent(projDir).Parent.FullName + @"\images\";

            var watch = System.Diagnostics.Stopwatch.StartNew();
            AsBitmap(imagesDir, args[0]);
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
        }

        private static void AsBitmap(string path, string filename)
        {
        
            var filenameNoExt = Path.GetFileNameWithoutExtension(filename);
            using (var img = Image.FromFile(path + filename))
            {
                var bmp = (Bitmap)img;
                for (int i = 0; i < bmp.Width; i++)
                {
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        Color pixel = bmp.GetPixel(i, j);
                        int t = (pixel.R + pixel.G + pixel.B) / 3;

                        switch (t)
                        {
                            case < 32:
                                bmp.SetPixel(i, j, Color.FromArgb(255, 0, 0, 128 + t * 4));
                                break;
                            case < 96:
                                bmp.SetPixel(i, j, Color.FromArgb(255, 0, (t - 32) * 4, 255));
                                break;

                            case < 160:
                                bmp.SetPixel(i, j, Color.FromArgb(255, (t - 96) * 4, 255, 255 - (t - 96) * 4));
                                break;

                            case < 224:
                                bmp.SetPixel(i, j, Color.FromArgb(255, 255, 255 - (t - 160) * 4, 0));
                                break;
                            default:
                                bmp.SetPixel(i, j, Color.FromArgb(255, 255 - (t - 224) * 4, 0, 0));
                                break;
                        }
                    }
                }
                var fs = File.Create(path + filenameNoExt + "_bmp.png");
                bmp.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
            }
            
        }

        private static void AsSIMD(string path, string filename)
        {

            using (var fs = File.OpenRead(path))
            {
                for(int i = 0; i < fs.Length; i += 32)
                {
                    byte[] buff = new byte[8 * 4]; // #pixels that fit into 256 bits * size of a pixel in bytes
                    fs.ReadExactly(buff, i, 32);
                    //Vector256<byte> data = Vector256.Create(); ;
                }
                
                
            }
        }
    }
}
