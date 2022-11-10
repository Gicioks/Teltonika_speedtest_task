using System;
using System.Diagnostics;

//trash playground, ignore
namespace Teltonika_speedtest_task
{
    internal class Program
    {
        private static Json_Reader json_reader = null;
        private static Printer consolePrinter = null;
        private static Printer resultPrinter = null;

        static void Main(string[] args)
        {
            json_reader = new Json_Reader();
            consolePrinter = new Printer(writeToFile: false);
            resultPrinter = new Printer();

            Console.WriteLine("Speedtest started...");

            Speedtest();

            consolePrinter.PrintSuccess("Speedtest finished.");
            Console.WriteLine(string.Format("Results saved in '{0}' file.", resultPrinter.GetFilePath()));
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void Speedtest()
        {
            int testCount = json_reader.GetTestCount();

            for (int i = 0; i < json_reader.GetDeviceArray().Count; i++)
            {
                string deviceName = json_reader.GetDeviceName(i);
                string deviceIP = json_reader.GetDeviceIP(i);
                string devicePasswd = json_reader.GetDevicePasswd(i);
                double passValue = json_reader.GetPassSpeedValue(i);
                
                string status = "";
                string resultVal = "";

                Console.WriteLine(string.Format("Connecting to {0} ({1})...", deviceIP, deviceName));
                SSH_client ssh = new SSH_client(ip: deviceIP, password: devicePasswd);
                
                //if connected to right device
                if (ssh.IsConnected() && string.Equals(ssh.GetDevice(), deviceName, StringComparison.CurrentCultureIgnoreCase))
                {
                    consolePrinter.PrintSuccess("Success.");
                    Console.WriteLine("Running speedtest...");

                    double speed_bps = SpeedTestAverage(testCount);
                    resultVal = ResultSpeedString(speed_bps);

                    if (speed_bps >= passValue)
                    {
                        status = "Passed";
                    }
                    else
                    {
                        status = "Failed";
                    }
                }
                else if (ssh.IsConnected())
                {
                    consolePrinter.PrintSuccess("Success.");
                    consolePrinter.PrintWarning("Incorrect device name given, check config file. Skipping...");

                    status = "Failed";
                    resultVal = "Incorrect device name provided in config file";
                }
                else
                {
                    consolePrinter.PrintError("Failed.");
                    status = "Failed";
                    resultVal = "Connection not established";
                }

                resultPrinter.PrintResults(deviceName, status, resultVal);
            }
        }

        static double SpeedTestAverage(int count)
        {        
            double averageSpeed = 0;
            for (int i = 0; i < count; i++)
            {
                double tmp = SpeedTestRun();
                averageSpeed += tmp;

                consolePrinter.PrintGray(string.Format("{0}: {1} bps", i + 1, Math.Floor(tmp)));
            }
            averageSpeed = averageSpeed / count;

            return averageSpeed;
        }

        static double SpeedTestRun()
        {
            Stopwatch time = new Stopwatch();

            byte[] bytes;
            using (var client = new System.Net.WebClient())
            {
                time.Start();
                bytes = client.DownloadData("http://speedtest.ftp.otenet.gr/files/test100Mb.db");
                time.Stop();
            }
            double speed = bytes.LongLength * 8 / time.Elapsed.TotalSeconds; //bits per sec

            return speed;
        }

        static string ResultSpeedString(double value)
        {
            string stringVal = null;
            if (value > 1048576)
            {
                stringVal = bps_To_Mbps(value) +  " Mbps";
            }
            else if (value > 1024)
            {
                stringVal = bps_To_kbps(value) + " kbps";
            }
            else
            {
                stringVal = value + " bps";
            }

            return stringVal;
        }

        static double bps_To_kbps(double bps)
        {
            return Math.Round(bps / 1024, 2);
        }
        static double bps_To_Mbps(double bps)
        {
            return Math.Round(bps / 1048576, 2);
        }
    }
}
