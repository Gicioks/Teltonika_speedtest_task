using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teltonika_speedtest_task
{
    internal class Json_Reader
    {
        private string json_path = null;
        private int test_count = 0;

        private JObject json = null;

        public Json_Reader(string json_path="..\\..\\config.json")
        {
            this.json_path = json_path;
            ReadConfig();

            test_count = (int) json["test_count"];
        }

        private void ReadConfig()
        {
            StreamReader sr = new StreamReader(json_path);
            JsonTextReader reader = new JsonTextReader(sr);

            json = (JObject) JToken.ReadFrom(reader);
        }

        public int GetTestCount()
        {
            return test_count;
        }

        public JArray GetDeviceArray()
        {
            JArray array = new JArray();

            array = (JArray) json["devices"];

            return array;
        }

        public JObject GetDevice(int index)
        {
            JObject device = new JObject();

            device = (JObject) GetDeviceArray()[index];

            return device;
        }

        public string GetDeviceName(int index)
        {
            return (string)GetDeviceArray()[index]["device"];
        }

        public string GetDeviceIP(int index)
        {
            return (string)GetDeviceArray()[index]["ip"];
        }
        public string GetDevicePasswd(int index)
        {
            return (string)GetDeviceArray()[index]["password"];
        }

        public string GetDeviceSpeedPass(int index)
        {
            return (string)GetDeviceArray()[index]["pass_test"];
        }

        /// <summary>
        /// Returns speed pass value
        /// </summary>
        /// <param name="index"></param>
        /// <returns>value in Bps</returns>
        public double GetPassSpeedValue(int index)
        {
            string stringValue = GetDeviceSpeedPass(index);

            string[] splitValue = stringValue.Split(' ');

            if (string.Equals(splitValue[1], "kbps", StringComparison.CurrentCultureIgnoreCase))
            {
                return kbps_To_bps (double.Parse(splitValue[0]));
            }
            else if (string.Equals(splitValue[1], "mbps", StringComparison.CurrentCultureIgnoreCase))
            {
                return Mbps_To_bps(double.Parse(splitValue[0]));
            }

            return -1;
        }
        private double kbps_To_bps(double kBps)
        {
            return Math.Round(kBps * 1024, 2);
        }
        private double Mbps_To_bps(double MBps)
        {
            return Math.Round(MBps * 1048576, 2);
        }
    }
}
