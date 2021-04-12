using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace Server
{
    public class Client
    {
        public TCP tcp;

        public int id;
        public Player player;

        public Client(int _clientId)
        {
            id = _clientId;
            tcp = new TCP(id);
        }
    }

    public class TCP
    {
        private int id;
        public TcpClient socket;
        private NetworkStream stream;

        private int bufferSize = 1024;
        private byte[] buffer;

        public TCP(int _id)
        {
            id = _id;
        }

        public void Connect(TcpClient _socket)
        {
            socket = _socket;
            socket.ReceiveBufferSize = bufferSize;
            socket.SendBufferSize = bufferSize;
            stream = socket.GetStream();

            buffer = new byte[bufferSize];

            stream.BeginRead(buffer, 0, bufferSize, ReceiveCallback, null);

            // Send login packet to client
        }

        public void SendData(Packet _packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to player {id} via TCP: {_ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int byteLength = stream.EndRead(_result);
                if (byteLength == 0)
                {
                    Server.clients[id].tcp.Disconnect();
                    return;
                }

                byte[] data = new byte[byteLength];
                Array.Copy(buffer, data, byteLength);

                ThreadManager.ExecuteOnMainThread(() =>
                {
                    ClientPacket.Handle(new Packet(data));
                });

                stream.BeginRead(buffer, 0, bufferSize, ReceiveCallback, null);
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error receiving TCP data: {_ex}");
                Server.clients[id].tcp.Disconnect();
            }
        }

        public void Disconnect()
        {
            socket.Close();
            stream = null;
            buffer = null;
            socket = null;
        }
    }
}