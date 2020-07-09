using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CrystalAlchemistUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Waiting 3 seconds...");
                Thread.Sleep(3 * 1000);

                DirectoryInfo current = new DirectoryInfo(Directory.GetCurrentDirectory());
                FileInfo[] file = current.GetFiles("CrystalAlchemist.zip");

                if (file.Length > 0)
                {
                    Console.WriteLine("Unzip File: "+file[0].FullName);

                    FastZip fastZip = new FastZip();
                    string fileFilter = "-CrystalAlchemistUpdater.exe;-ICSharpCode.SharpZipLib.dll";
                    fastZip.ExtractZip(file[0].FullName, current.FullName, fileFilter);

                    Thread.Sleep(1000);
                    Console.WriteLine("Completed: " + file[0].FullName);

                    file[0].Delete();
                    file = current.GetFiles("CrystalAlchemist.exe");

                    if (file.Length > 0)
                    {
                        Console.WriteLine("Start Game in 3 seconds...");
                        Thread.Sleep(3 * 1000);

                        Process.Start(file[0].FullName);
                    }
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
