using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Server
{
    public class Server
    {
        public static int maxPlayers;
        public static int port; 

        public static Client[] clients;
        private static TcpListener tcpListener;

        public static bool Start(int _maxPlayers, int _port)
        {
            maxPlayers = _maxPlayers;
            port = _port;

            Debug.Log("Starting server...");

            clients = new Client[maxPlayers];
            for (int i = 0; i < maxPlayers; ++i)
                clients[i] = new Client(i);

            try
            {
                tcpListener = new TcpListener(IPAddress.Any, port);
                tcpListener.Start();
                tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
            }
            catch (Exception _ex)
            {
                Debug.LogError($"Failed to start server on port {port}: {_ex}");
                return false;
            }

            Debug.Log($"Server started on port {port}...");
            return true;
        }

        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient client = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);

            Debug.Log($"Incoming connection from {client.Client.RemoteEndPoint}...");

            for (int i = 1; i <= maxPlayers; i++)
            {
                if (clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(client);
                    return;
                }
            }

            Debug.Log($"{client.Client.RemoteEndPoint} failed to connect: Server full!");
        }

        public static void Stop()
        {
            tcpListener.Stop();
        }
    }
}