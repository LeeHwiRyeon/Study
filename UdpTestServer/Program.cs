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
            IPHostEntry IPHost = Dns.GetHostByName(Dns.GetHostName());
            Console.WriteLine("현재 PC AddressList");
            foreach (var address in IPHost.AddressList) {
                Console.WriteLine(address.ToString());
            }

            PortInit:
            Console.WriteLine("서버 포트 입력");
            int.TryParse(Console.ReadLine(), out int port);
            if (port <= 0) {
                Console.WriteLine("잘 못 된 포트");
                Console.WriteLine("1 ~ 65535 범위 안에서 지정하세요.");
                goto PortInit;
            }

            Console.WriteLine("Udp 서버 대기시작");
            var localEP = new IPEndPoint(IPAddress.Any, port);
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
