using Photon.Pun;
using TMPro;
using UnityEngine;
using System.Collections;

public class LobbyUIControl : MonoBehaviourPun
{
    [Header("Overlay Panels")]
    public GameObject createRoomPanel;
    public GameObject joinRoomPanel;
    public GameObject MPweaponpanel;
    public GameObject Playerlistpanel;
    public GameObject Noroompopup;

    [Header("Inputs")]
    public TMP_InputField roomNameInput;
    public TMP_Dropdown maxPlayersDropdown;
    public TMP_InputField passwordInput;

    public TMP_InputField joinRoomNameInput;
    public TMP_InputField joinPasswordInput;

    public LobbyManager lobbyManager;

    public TMP_InputField Playername;

    // =========================
    // Create Room
    // =========================

    public void OpenCreateRoom()
    {
        createRoomPanel.SetActive(true);
    }

    public void CloseCreateRoom()
    {
        createRoomPanel.SetActive(false);
    }

    public void OnCreateRoomClicked()
    {
        string roomName = roomNameInput.text;

        if (string.IsNullOrEmpty(roomName))
            return;

        byte maxPlayers = (byte)
            int.Parse(maxPlayersDropdown.options[maxPlayersDropdown.value].text);

        string password = passwordInput.text;

        lobbyManager.CreateRoom(roomName, maxPlayers, password);

        createRoomPanel.SetActive(false);
    }

    // =========================
    // Join Room
    // =========================

    public void OpenJoinRoom()
    {
        joinRoomPanel.SetActive(true);
    }

    public void CloseJoinRoom()
    {
        joinRoomPanel.SetActive(false);
    }

    public void OnJoinRoomClicked()
    {
        string roomName = joinRoomNameInput.text;
        string password = joinPasswordInput.text;

        if (string.IsNullOrEmpty(roomName))
            return;

        lobbyManager.JoinRoomByName(roomName, password);

        joinRoomPanel.SetActive(false);
    }

    // =========================
    // Overlays
    // =========================

    public void ShowPlayerList()
    {
        Playerlistpanel.SetActive(true);
    }

    public void ClosePlayerList()
    {
        Playerlistpanel.SetActive(false);
    }

    public void ShowMPWeaponPanel()
    {
        MPweaponpanel.SetActive(true);
    }

    public void CloseMPWeaponPanel()
    {
        MPweaponpanel.SetActive(false);
    }

    // =========================
    // Start Game
    // =========================

    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        PhotonNetwork.LoadLevel(2);
    }

    // =========================
    // No Room Popup
    // =========================

    IEnumerator ShowPopup()
    {
        Noroompopup.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        Noroompopup.SetActive(false);
    }

    public void TriggerPopup()
    {
        StartCoroutine(ShowPopup());
    }


    public void ShowMPweaponpanel()
   {
    ShowMPWeaponPanel();
   }

public void Triggerpopup()
{
    TriggerPopup();
}

     public void OnLeavecliked()
    {
        PhotonNetwork.LeaveRoom();
    }


    void OnDisable()
    {
        createRoomPanel.SetActive(false);
        joinRoomPanel.SetActive(false);
        MPweaponpanel.SetActive(false);
        Playerlistpanel.SetActive(false);
        
    }

}