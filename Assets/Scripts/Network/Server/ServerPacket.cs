using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerPacket
{
    public const int WELCOME = 0;

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
            case WELCOME:
                HandleWelcome(_packet);
                break;
            default:
                break;
        }
    }

    #region Packets
    public static void SendWelcome(int _toClient)
    {
        Packet packet = new Packet(WELCOME);
        SendTCPData(_toClient, packet);
    }
    
    private static void HandleWelcome(Packet _packet)
    {
        // Disable join menu and enable game UI and map
        UIManager.Instance.canvasJoin.gameObject.SetActive(false);
        UIManager.Instance.canvasGameUI.gameObject.SetActive(true);
        Game.Instance.map.gameObject.SetActive(true);
    }
    #endregion
}
