using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Client
{
    public class Client
    {
        public static TCP tcp;

        public static void Connect(IPAddress _ipAddress, int _port)
        {
            tcp = new TCP(_ipAddress, _port);
        }

        public static void Disconnect()
        {
            tcp.socket.Close();
            Debug.Log("Disconnected from server.");
        }
    }

    public class TCP
    {
        public TcpClient socket;
        private NetworkStream stream;

        private int bufferSize = 1024;
        private byte[] buffer;

        public TCP(IPAddress _ipAddress, int _port)
        {
            socket = new TcpClient();
            socket.ReceiveBufferSize = bufferSize;
            socket.SendBufferSize = bufferSize;

            buffer = new byte[bufferSize];

            socket.BeginConnect(_ipAddress, _port, ConnectCallback, socket);
        }

        private void ConnectCallback(IAsyncResult _result)
        {
            socket.EndConnect(_result);

            if (!socket.Connected)
                return;

            stream = socket.GetStream();
            stream.BeginRead(buffer, 0, bufferSize, ReceiveCallback, null);
        }

        public void SendData(Packet _packet)
        {
            try
            {
                if (socket != null)
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to server via TCP: {_ex}");
            }
        }

        /// <summary>Reads incoming data from the stream.</summary>
        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int byteLength = stream.EndRead(_result);
                if (byteLength == 0)
                {
                    Disconnect();
                    return;
                }

                byte[] data = new byte[byteLength];
                Array.Copy(buffer, data, byteLength);

                ThreadManager.ExecuteOnMainThread(() =>
                {
                    ServerPacket.Handle(new Packet(data));
                });

                stream.BeginRead(buffer, 0, bufferSize, ReceiveCallback, null);
            }
            catch
            {
                Disconnect();
            }
        }

        private void Disconnect()
        {
            stream = null;
            buffer = null;
            socket = null;
        }
    }
}