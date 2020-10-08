using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AliPayServer
{
    class Program
    {
        
        static void Main(string[] args)
        {
            TCPServer tCPServer = new TCPServer();
            tCPServer.Start();
            Console.ReadLine();
        }
    }
}
