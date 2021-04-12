using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientPacket
{
    private static void SendTCPData(Packet _packet)
    {
        Client.Client.tcp.SendData(_packet);
    }

    // Handles packets that come from server
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
