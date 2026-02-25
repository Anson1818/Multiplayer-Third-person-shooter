using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{

    public static event Action Onjoinlobby;
    // Called by Multiplayer button
    public void ConnectToPhoton()
    {
        if (PhotonNetwork.IsConnected)
            return;

        Debug.Log("Connecting to Photon...");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server");

        // IMPORTANT: Join Lobby, not a room
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Photon Lobby");
        Onjoinlobby.Invoke();
        // At this point:
        // - Enable Lobby UI
        // - Enable LobbyManager
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("Disconnected from Photon: " + cause);
    }
}
