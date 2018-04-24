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
        private string message;
        private bool keepAlive;

        public Server()
        {
            server = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
            server.Start();
        }
        public async void RunAsync()
        {

            keepAlive = true;

            while (keepAlive)
            {

                await Task.Run(() =>
                {
                    AcceptClient();
                });
                
                message = await Task<string>.Run(() =>
                {
                    string clientMessage = client.Recieve();
                    return clientMessage;
                });
                //message = taskClientReceiveMessage.Result;

                await Task.Run(() =>
                {
                    Respond(message);
                });
                Task.WaitAll();
            }

            //AcceptClient();
            //client.Recieve());
            //Respond(message));
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
