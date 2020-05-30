using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Find_Lines_in_File
{
    /// <summary>
    /// Copies lines from a text file containing search phrases, paste those lines into a new text file.
    /// </summary>
    /// <remarks>
    /// 1st parameter is full path of a file that has phrases to be searched, one per line.
    /// 2nd param is file to search in, e.g. a log file.
    /// 3rd param is output file name. If not supplied, default is out.txt
    /// </remarks>
    class Program
    {
        static void Main(string[] args)
        {
            var exeName = Process.GetCurrentProcess().MainModule.FileName;
            var exeDir = Directory.GetCurrentDirectory();
            //var exeName = System.AppDomain.CurrentDomain.FriendlyName;

            using (var p = new Process()) //https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandardinput?view=netcore-3.1
            {
                
                p.StartInfo.FileName = "cmd.exe";           //
                p.StartInfo.UseShellExecute = false;        //run process in same cmd window
                p.StartInfo.RedirectStandardInput = true;   //std input on cmd window will be passed on to in process
                p.Start();
                //p.WaitForExit();
                Thread.Sleep(1000);                         //to prevent cmd title printing AFTER cw() below
                cw("");
                
                string searchPath = "";
                var srcFilePath =  "";
                var outputPath = Path.Combine(exeDir, "out.txt");
                var fileExists = false;
                //string inputStd;

                while (!fileExists)
                {
                    cw("Enter path for file containing search phrases:");
                    searchPath = cr();
                    fileExists = File.Exists(searchPath);
                    if (fileExists)
                    {
                        fileExists = false; //make ready for upcoming while()
                        break;
                    }
                    cw("File doesn't exist !");
                }

                while (!fileExists)
                {
                    cw("Enter path for file to search in:");
                    srcFilePath = cr();
                    fileExists = File.Exists(srcFilePath);
                    if (fileExists)
                    {
                        fileExists = false; //make ready for upcoming while()
                        break;
                    }
                    cw("File doesn't exist !");
                }

                cw("Enter path output file - if blank then default will be out.txt");
                var outputPathEntered = cr();
                outputPath = string.IsNullOrWhiteSpace(outputPathEntered) ? outputPath : outputPathEntered; //no while() coz out.txt will be created

                var searchLines = File.ReadAllLines(searchPath);
                var searchRaw = new List<string>(searchLines);
                var search = searchRaw.Select(l => l.Trim()).Where(l => !String.IsNullOrWhiteSpace(l)).ToList();
                var x = File.ReadAllLines(srcFilePath).Where(srcLine => search.Any(i => srcLine.Contains(i))).Select(o => o + Environment.NewLine);
                File.WriteAllLines(outputPath, x);
                cw($"done - check the file {outputPath}");
                cr();
            }


            //var p = new Process();
            //var startInfo = new ProcessStartInfo();
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //startInfo.FileName = "cmd.exe";
            //startInfo.Arguments = "/C NET USE F: /delete";
            //startInfo.RedirectStandardError = true;
            //startInfo.RedirectStandardInput = true;
            //startInfo.RedirectStandardOutput = true;
            //startInfo.UseShellExecute = false;
            //startInfo.CreateNoWindow = true;
            //startInfo.Arguments = exeName;
            //p.StartInfo = startInfo;
            //p.Start();

            //ManualResetEvent manualResetEvent = new ManualResetEvent(false);
            //manualResetEvent.WaitOne();
            //manualResetEvent.Set();
        }
        static Action<string> cw = x => Console.WriteLine(x);
        static Func<string> cr = () => Console.ReadLine();
    }
}
