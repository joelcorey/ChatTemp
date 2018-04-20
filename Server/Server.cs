using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        public static Client client;
        TcpListener server;
        bool keepAlive;
        private string message;

        public Server()
        {
            server = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
            server.Start();
        }
        public void Run()
        {

            keepAlive = true;

            while (keepAlive)
            {

                Task taskAccecptClient = Task.Factory.StartNew(() =>
                {
                    AcceptClient();
                });
                taskAccecptClient.Wait();

                // continue here ..
                Task<string> taskClientReceiveMessage = Task<string>.Factory.StartNew(() => {
                    string clientMessage = client.Recieve();
                    return clientMessage;
                });
                message = taskClientReceiveMessage.Result;
                
                Task taskClientRespondMessage = Task.Factory.StartNew(() => {
                    Respond(message);
                });
            }
        }

        private string ClientReceive()
        {
            return client.Recieve();
        }

        private void AcceptClient()
        {
            TcpClient clientSocket = default(TcpClient);
            clientSocket = server.AcceptTcpClient();
            Console.WriteLine("Connected");
            NetworkStream stream = clientSocket.GetStream();
            client = new Client(stream, clientSocket);
 
        }
        private void Respond(string body)
        {
             client.Send(body);
        }
    }
}
