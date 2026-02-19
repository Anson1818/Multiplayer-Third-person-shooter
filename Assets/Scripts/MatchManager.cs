using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class MatchManager : MonoBehaviourPunCallbacks
{
    public static MatchManager Instance;

    public float matchDuration = 300f; // 5 minutes
    float remainingTime;

    MPUIcontrol mpui;

    bool matchRunning;

    List<ScoreEntry> finalScores = new List<ScoreEntry>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            remainingTime = matchDuration;
            matchRunning = true;
        }

       mpui=GetComponent<MPUIcontrol>();
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (!matchRunning)
            return;

        remainingTime -= Time.deltaTime;

        // Sync time to everyone
        photonView.RPC(nameof(RPC_UpdateTime), RpcTarget.All, remainingTime);

        if (remainingTime <= 0)
        {
            EndMatch();
        }
    }

    void EndMatch()
    {
        matchRunning = false;
        CalculateFinalScores();

        photonView.RPC(nameof(RPC_EndMatch), RpcTarget.All);

    }

    [PunRPC]
    void RPC_UpdateTime(float time)
    {
        remainingTime = time;
    }

    [PunRPC]
    void RPC_EndMatch()
    {
        matchRunning = false;
        Debug.Log("Match Ended");


         // Disable local player controls
        if (MultiplayerLevelController.instance != null)
        {
            MultiplayerLevelController.instance.DisableLocalPlayer();
        }

       mpui.Showmatchoverpanel();    
       

       if (PhotonNetwork.IsMasterClient)
        {
     foreach (Player player in PhotonNetwork.PlayerList)
    {
        int kills = player.CustomProperties.ContainsKey(PlayerProterties.Kills)
            ? (int)player.CustomProperties[PlayerProterties.Kills]
            : 0;

        int deaths = player.CustomProperties.ContainsKey(PlayerProterties.Deaths)
            ? (int)player.CustomProperties[PlayerProterties.Deaths]
            : 0;

        Debug.Log($"{player.NickName} | Kills: {kills} | Deaths: {deaths}");
    }
       }

    
       }

    public float GetRemainingTime()
    {
        return remainingTime;
    }

    public bool IsMatchRunning()
    {
        return matchRunning;
    }


    void CalculateFinalScores()
{
    finalScores.Clear();

    foreach (Player player in PhotonNetwork.PlayerList)
    {
        int kills = 0;
        int deaths = 0;

        if (player.CustomProperties.TryGetValue(PlayerProterties.Kills, out object k))
            kills = (int)k;

        if (player.CustomProperties.TryGetValue(PlayerProterties.Deaths, out object d))
            deaths = (int)d;

        finalScores.Add(new ScoreEntry(player, kills, deaths));
    }

    // Sort by kills DESC
    finalScores.Sort((a, b) => b.kills.CompareTo(a.kills));
}

}
