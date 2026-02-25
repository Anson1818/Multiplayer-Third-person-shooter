using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;


public class LobbyManager : MonoBehaviourPunCallbacks
{
    public LobbyUIControl lobbyui;
    // =========================
    // TEMP STORAGE FOR JOIN
    // =========================
    private string pendingJoinPassword;
    

    // =========================
    // CREATE ROOM
    // =========================
    public void CreateRoom(string roomName, byte maxPlayers, string password)
    {
        if (!PhotonNetwork.InLobby)
        {
            Debug.LogWarning("Not in lobby yet.");
            return;
        }

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayers;

        Hashtable roomProps = new Hashtable();
        roomProps["rn"] = roomName;
        roomProps["pw"] = password;

        options.CustomRoomProperties = roomProps;

        // Only room name visible in lobby (optional)
        options.CustomRoomPropertiesForLobby = new string[] { "rn" };

        PhotonNetwork.CreateRoom(roomName, options);
        Debug.Log("Creating room: " + roomName);
    }

    // =========================
    // JOIN ROOM (BY NAME)
    // =========================
    public void JoinRoomByName(string roomName, string enteredPassword)
    {
        if (!PhotonNetwork.InLobby)
        {
            Debug.LogWarning("Not in lobby");
            return;
        }

        pendingJoinPassword = enteredPassword;
        PhotonNetwork.JoinRoom(roomName);

        Debug.Log("Attempting to join room: " + roomName);
    }
     public override void OnJoinedRoom()
{
    Debug.Log("Joined room successfully");
    PhotonNetwork.NickName=lobbyui.Playername.text;
     Debug.Log(PhotonNetwork.NickName);

    // Password validation only for joiners
    if (!PhotonNetwork.IsMasterClient)
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("pw", out object pw))
        {
            string roomPassword = pw as string;

            if (!string.IsNullOrEmpty(roomPassword) &&
                roomPassword != pendingJoinPassword)
            {
                Debug.Log("Wrong password. Leaving room.");
                PhotonNetwork.LeaveRoom();
                return;
            }
        }
    }

    Debug.Log("Password OK. Showing weapon panel.");

    // 🔑 THIS MUST HAPPEN FOR BOTH CREATOR & JOINERS
   
    lobbyui.ShowMPweaponpanel();
      
}

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.JoinLobby();
        Debug.Log("rejoined lobby");
        
    }

    public void Joinrandonroom()
    {
         PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        lobbyui.Triggerpopup();
    }
    
        public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Create room failed: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Join room failed: " + message);
    }
}
