using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine.SceneManagement;


public class MPUIcontrol : MonoBehaviourPunCallbacks
{

    public List<TextMeshProUGUI> playerrows;
    public TextMeshProUGUI Killcountdisplay;
    public GameObject deadpanel;

     public GameObject Matchoverpanel;

     public GameObject Scoreboard;
    

   public void UpdateKillcount(int count)
    {
        Killcountdisplay.text=count.ToString();
    }
      
  public override void OnPlayerPropertiesUpdate( Player targetPlayer,ExitGames.Client.Photon.Hashtable changedProps)
   {
    if (!targetPlayer.IsLocal)
        return;

    if (changedProps.ContainsKey(PlayerProterties.Kills))
    {
        Killcountdisplay.text =  changedProps[PlayerProterties.Kills].ToString();
    }
}

  public  void Showdeadpanel()
    {
         deadpanel.SetActive(true);
    }

   public  void Hidedeadpanel()
    {
        deadpanel.SetActive(false);
    }

   public void Showmatchoverpanel()
    {
        Matchoverpanel.SetActive(true);
        StartCoroutine(finalscoreboard());
    }
    public void Hidematchoverpanel()
    {
        Matchoverpanel.SetActive(false);
    }


     public void ShowFinalScoreboard()
    {
        Scoreboard.SetActive(true);

       

        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Length && i < playerrows.Count; i++)
        {
            Player player = players[i];

            int kills = player.CustomProperties.ContainsKey(PlayerProterties.Kills)
                ? (int)player.CustomProperties[PlayerProterties.Kills]
                : 0;

            int deaths = player.CustomProperties.ContainsKey(PlayerProterties.Deaths)
                ? (int)player.CustomProperties[PlayerProterties.Deaths]
                : 0;

            playerrows[i].text =
                $"{player.NickName}  |  {kills}  |  {deaths}";
        }
    }

     IEnumerator finalscoreboard()
    {
        yield return new WaitForSecondsRealtime(2);
        Hidematchoverpanel();
        ShowFinalScoreboard();
    }

    public void Onreturnlobbyclicked()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
    }

}
