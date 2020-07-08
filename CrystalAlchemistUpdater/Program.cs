using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Diagnostics;
using System.IO;

namespace CrystalAlchemistUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                DirectoryInfo current = new DirectoryInfo(Directory.GetCurrentDirectory());
                FileInfo[] file = current.GetFiles("Patch.zip");

                if (file.Length > 0)
                {
                    Console.WriteLine("Unzip File "+file[0].FullName);
                    FastZip fastZip = new FastZip();
                    string fileFilter = null;
                    fastZip.ExtractZip(file[0].FullName, current.FullName, fileFilter);

                    file[0].Delete();

                    Console.WriteLine("Start Game");
                    file = current.GetFiles("Crystal Alchemist.exe");
                    if (file.Length > 0) Process.Start(file[0].FullName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
        }
    }
}
