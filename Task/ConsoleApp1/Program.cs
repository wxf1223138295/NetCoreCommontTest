using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

          string GoogleInstalledDefaultPath = $"C:\\Users\\{Environment.UserName}\\AppData\\Local\\Google\\Chrome\\Application\\chrome.exe";


          Console.WriteLine(GoogleInstalledDefaultPath);

        var re = Console.ReadLine();

            var uu=System.Environment.UserName;

            var path = @"C:\Users\pelag\AppData\Local\Google\Chrome\Application\chrome.exe";
            try
            {
                var info = new ProcessStartInfo()
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    Arguments = re,
                    FileName = path
                };
                Process.Start(re);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }


            Console.ReadKey();
        }
    }
}
