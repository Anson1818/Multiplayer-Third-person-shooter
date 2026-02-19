using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyUIControl : MonoBehaviourPun
{
     public GameObject lobbyMainPanel;
    public GameObject createRoomPanel;

     public TMP_InputField roomNameInput;
    public TMP_Dropdown maxPlayersDropdown;
    public TMP_InputField passwordInput;

    public LobbyManager lobbyManager;


    public GameObject joinRoomPanel;
    public TMP_InputField joinRoomNameInput;
    public TMP_InputField  joinPasswordInput;
    public GameObject MPweaponpanel;
    public GameObject Playerlistpanel;

    public TMP_InputField Playername;

    public GameObject Noroompopup;

    

    public void OnCreateRoomClicked()
    {
        string roomName = roomNameInput.text;

        if (string.IsNullOrEmpty(roomName))
        {
            Debug.Log("Room name is required");
            return;
        }

        // Example dropdown values: 2, 4, 8
        byte maxPlayers = (byte)(
            int.Parse(maxPlayersDropdown.options[maxPlayersDropdown.value].text)
        );

        string password = passwordInput.text;

        lobbyManager.CreateRoom(roomName, maxPlayers, password);

        // Close panel (Photon callback will confirm)
          CloseCreateRoom();
    }

     public void Showplayerlist()
    {
        Playerlistpanel.SetActive(true);
    }
     public void Closeplayerlist()
    {
        Playerlistpanel.SetActive(false);
    }

    public void OnCancelClicked()
    {
       CloseCreateRoom();
    }

    public void OpenCreateRoom()
    {
       
        createRoomPanel.SetActive(true);
    }

    public void CloseCreateRoom()
    {
        createRoomPanel.SetActive(false);
       
    }

    public void BackToHome()
    {
        // Later: Photon disconnect if needed
        lobbyMainPanel.SetActive(false);
    }

    public void OpenJoinRoom()
{
    joinRoomPanel.SetActive(true);
}

  public void ShowNoRoompopup()
    {
        Noroompopup.SetActive(true);
    }

    public void HideNoRoompopup()
    {
        Noroompopup.SetActive(false);
    }

public void CloseJoinRoom()
{
    joinRoomPanel.SetActive(false);
}
   public void ShowMPweaponpanel()
 {
     MPweaponpanel.SetActive(true);       
    }
 public void CloseMPweaponpanel()
    {
        MPweaponpanel.SetActive(false);
    }   

public void OnJoinRoomClicked()
{
    string roomName = joinRoomNameInput.text;
    string password = joinPasswordInput.text;

    if (string.IsNullOrEmpty(roomName))
    {
        Debug.Log("Room name required");
        return;
    }

    lobbyManager.JoinRoomByName(roomName, password);
    CloseJoinRoom();
}

  public void StartGame()
{
    if (!PhotonNetwork.IsMasterClient)
        return;

    
    PhotonNetwork.LoadLevel(2);
}

   public void Onleaveclicked()
    {
        PhotonNetwork.LeaveRoom();
        Debug.Log("leaving room");
    }

    IEnumerator Showpopup()
    {
        ShowNoRoompopup();
        yield return new WaitForSecondsRealtime(2);
        HideNoRoompopup();
    }

   public void Triggerpopup()
    {
        StartCoroutine(Showpopup());
    }

}
