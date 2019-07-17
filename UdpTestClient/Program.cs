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
            var msg = "TestMsg";
            var ip = "127.0.0.1";
            var port = 7777;
            using (var client = new UdpClient()) {
                Start:
                Console.WriteLine("IP 입력(Q 입력시 종료)");
                ip = Console.ReadLine();
                if (ip == "q" || ip == "Q") {
                    // 종료
                } else {
                    try {
                        var point = new IPEndPoint(IPAddress.Parse(ip), port);
                        Console.WriteLine("메시지 입력");
                        msg = Console.ReadLine();
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
