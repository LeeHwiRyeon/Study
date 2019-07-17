using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace UdpTestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var localEP = new IPEndPoint(IPAddress.Any, 7777);
            var udp = new UdpClient(localEP);
            udp.BeginReceive(ReceiveCallback, udp);

            while (true) {
                Thread.Sleep(2000);
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            var udp = ar.AsyncState as UdpClient;
            var e = new IPEndPoint(0, 0);

            var receiveBytes = udp.EndReceive(ar, ref e);
            var receiveString = Encoding.ASCII.GetString(receiveBytes);

            Console.WriteLine($"[Received] IP({e.Address}:{e.Port}), msg({receiveString})");
            udp.BeginReceive(ReceiveCallback, udp);
        }
    }
}
