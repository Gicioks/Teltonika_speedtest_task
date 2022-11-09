using Colorful;
using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace Teltonika_speedtest_task
{
    internal class Printer
    {
        private string path = null;

        public Printer(bool writeToFile=true, string path="..\\..\\results.txt")
        { 
            if (writeToFile)
            {
                this.path = path;

                CreateFile();
            }
        }

        //Wanted fancy check but got lazy :'(
        private bool IsValidPath(string path)
        {
            Regex r = new Regex(@"^(([a-zA-Z]:)|(\))(\{1}|((\{1})[^\]([^/:*?<>""|]*))+)$");
            return r.IsMatch(path);
        }

        public string GetFilePath()
        {
            return Path.GetFullPath(path);
        }

        public void PrintResults(string device, string status, string info = "")
        {
            //Console
            Color statusColor = Color.Red;
            if (string.Equals(status, "passed", StringComparison.CurrentCultureIgnoreCase))
            {
                statusColor= Color.Green;
            }

            Colorful.Console.Write(device + " ", Color.Gray);
            Colorful.Console.Write(status + " ", statusColor);
            Colorful.Console.Write(info, Color.Gray);
            System.Console.WriteLine();

            //File
            string line = string.Format("{0} {1} {2}", device, status, info);

            AppendFile(line);
        }

        public void PrintError(string error)
        {
            Colorful.Console.WriteLine(error, Color.Red);
        }

        public void PrintWarning(string line)
        {
            Colorful.Console.WriteLine(line, Color.Yellow);
        }

        public void PrintSuccess(string line)
        {
            Colorful.Console.WriteLine(line, Color.Green);
        }

        public void PrintGray(string line)
        {
            Colorful.Console.WriteLine(line, Color.Gray);
        }

        public void PrintLine(string line)
        {
            System.Console.WriteLine(line);
        }

        private void CreateFile()
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private void AppendFile(string line)
        {
            if (line != null)
            {
                using (StreamWriter fileWriter = File.AppendText(path))
                {
                    fileWriter.WriteLine(line);
                }
            }
        }
    }
}
