using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;

namespace PingMaster
{
    internal class PingHelper
    {
        public int pingTimeout = 120;
        public PingHelper()
        {
        }

        public PingReply SinglePing(string address)
        {
            //string ipStr = "baidu.com";

            Ping ping = new Ping();

            PingOptions options = new PingOptions();
            options.DontFragment = true;

            string data = "PingMaster for ytvpn. Just a Ping Test";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            PingReply pr = null;
            try
            {
                pr = ping.Send(address, pingTimeout, buffer, options);
            }
            catch (System.Exception ex)
            {

                Debug.WriteLine(ex.StackTrace);
            }
            /*
            if (pr != null)
            {
                System.Console.WriteLine(string.Format("{0}-> ping:{1},timeout:{2}", address, pr.RoundtripTime, pr.Status));
            }
            else
            {
                System.Console.WriteLine(string.Format("{0}-> ping:{1},timeout:{2}", address, "null", "exception"));
            }
            */
            return pr;
        }
        public PingResult MultiPing(string address, int times)
        {
            PingReply reply;
            PingResult result = new PingResult();
            for (int i = 0; i < times; i++)
            {
                reply = SinglePing(address);
                if (reply != null && reply.Status == IPStatus.Success)
                {
                    result.addPing(reply.RoundtripTime);
                }
                else
                {
                    result.plusTimeout();
                }

            }
            return result;
        }
        internal class PingResult
        {
            public PingResult()
            {

            }
            List<long> PingList = new List<long>();

            public void addPing(long ping)
            {
                PingList.Add(ping);
            }

            public long GetAvePing()
            {
                long sumlong = 0;
                for (int i = 0; i < PingList.Count; i++)
                {
                    sumlong += PingList[i];
                }
                if (PingList.Count > 0)
                {
                    return sumlong / PingList.Count;
                }
                return 0;

            }

            int TimeOut = 0;
            public void plusTimeout()
            {
                TimeOut++;
            }
            public int GetTimeOut()
            {
                return TimeOut;
            }

        }
    }
}