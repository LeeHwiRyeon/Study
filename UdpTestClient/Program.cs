using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace UdpTestClient
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

            using (var client = new UdpClient()) {
                Start:
                Console.WriteLine("IP 입력(Q 입력시 종료)");
                var ip = Console.ReadLine();
                if (ip == "q" || ip == "Q") {
                    // 종료
                } else {
                    try {
                        PortInit:
                        Console.WriteLine("서버 포트 입력");
                        int.TryParse(Console.ReadLine(), out int port);
                        if (port <= 0) {
                            Console.WriteLine("잘 못 된 포트");
                            Console.WriteLine("0 ~ 65535 범위 안에서 지정하세요.");
                            goto PortInit;
                        }

                        var point = new IPEndPoint(IPAddress.Parse(ip), port);
                        Console.WriteLine("메시지 입력");
                        var msg = Console.ReadLine();
                        for (int i = 0; i < 5; i++) {
                            var datagram = Encoding.ASCII.GetBytes(msg + i);
                            client.Send(datagram, datagram.Length, point);
                            Console.WriteLine($"[Send] IP({ip}:{port})로 {datagram.Length} 바이트 전송");
                            Thread.Sleep(2000);
                        }
                    } catch (Exception e) {
                        Console.WriteLine("IP 주소가 잘 못 입력 됨.");
                    }
                    goto Start;
                }

                client.Close();
            }
        }
    }
}
