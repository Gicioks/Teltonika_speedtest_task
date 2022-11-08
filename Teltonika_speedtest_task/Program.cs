using Newtonsoft.Json.Linq;
using SpeedTest.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Teltonika_speedtest_task
{
    internal class Program
    {
        private static Json_Reader json_reader = null;
        static void Main(string[] args)
        {
            json_reader = new Json_Reader();

            Task.Run(async () =>
            {
                var speedtest = await SpeedTestClient.GetDownloadSpeed();
            }).Wait();
            
            //Console.WriteLine(SelectDevice());

            /*
            foreach (var device in json_reader.GetDeviceArray())
            {
                Console.WriteLine(device["device"].ToString());
                Console.WriteLine(device["ip"].ToString());
                Console.WriteLine(device["pass_test"].ToString());

                Console.WriteLine();
            }
            */
        }

        /*SpeedTestAverage(int count)
        {


            for (int i = 0; i < count; i++)
            {

            }


        }*/

        static async Task<string> SpeedTestRunAsync()
        {
            string speed = "";
            
            var speedtest = await SpeedTestClient.GetDownloadSpeed();

            return speed;
        }

        //static async Task<>

        static int SelectDevice()
        {
            int i = 0;
            foreach (var device in json_reader.GetDeviceArray())
            {
                SSH_client ssh = new SSH_client(IP: (string)device["ip"], Password: (string)device["password"]);

                if ((string)device["device"] == ssh.GetDevice())
                {
                    return i;
                }

                i++;
            }

            return -1;
        }
    }
}
