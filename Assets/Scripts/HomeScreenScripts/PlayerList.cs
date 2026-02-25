using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayerList : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI[] playerSlots;

    void Start()
    {
        RefreshList();
    }

    void RefreshList()
    {
      
        for (int i = 0; i < playerSlots.Length; i++)
        {
            playerSlots[i].text = "Waiting for player...";
        }

       
        Player[] players = PhotonNetwork.PlayerList;
        int count = Mathf.Min(players.Length, playerSlots.Length);

        for (int i = 0; i < count; i++)
        {
            playerSlots[i].text = players[i].NickName;
        }

        Debug.Log($"[PlayerList] Refreshed. Players: {players.Length}");
    }

    public override void OnJoinedRoom()
    {
        RefreshList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RefreshList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RefreshList();
    }
}
