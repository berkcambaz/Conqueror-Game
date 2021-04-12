using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerPacket
{
    private static void SendTCPData(int _toClient, Packet _packet)
    {
        Server.Server.clients[_toClient].tcp.SendData(_packet);
    }

    private static void SendTCPDataToAll(Packet _packet)
    {
        for (int i = 1; i <= Server.Server.maxPlayers; i++)
        {
            Server.Server.clients[i].tcp.SendData(_packet);
        }
    }

    private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
    {
        for (int i = 1; i <= Server.Server.maxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.Server.clients[i].tcp.SendData(_packet);
            }
        }
    }

    // Handles packets that come from clients
    public static void Handle(Packet _packet)
    {
        int id = _packet.ReadInt();
        switch (id)
        {
            default:
                break;
        }
    }

    #region Packets
    #endregion
}
