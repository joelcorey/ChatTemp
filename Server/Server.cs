﻿using System;
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

        public Server()
        {
            server = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
            server.Start();
        }
        public void Run()
        {
            AcceptClient();

            Task taskAccecptClient = Task<string>.Factory.StartNew(() => {
                string message = ClientReceive();
                return message;
            });

            //string message = ClientReceive();
            //string message = client.Recieve();
            Respond(message);
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
