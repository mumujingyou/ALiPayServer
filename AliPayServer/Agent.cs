using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AliPayServer
{
    class Agent
    {
        TcpClient client;
        public Agent(TcpClient tcpClient)
        {
            client = tcpClient;
            Receive();
        }

        private async void Receive()
        {
            try
            {
                byte[] buffer = new byte[1024];
                int length = await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
                string str = Encoding.UTF8.GetString(buffer, 0, length);
                if (str == "pay")
                {
                    AliPay.Instance.Pay(this);
                }
               
            }
            catch (Exception e)
            {
                Console.WriteLine($"接收异常:{e.Message}");
                return;
            }
            Receive();
        }

        public async void Send(string str) {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(str);
                await client.GetStream().WriteAsync(data,0,data.Length);
                Console.WriteLine($"发送的数据:{str}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"发送异常:{e.Message}");
                return;
            }
        
        }
    }
}
