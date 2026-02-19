using Photon.Pun;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using System.Collections.Generic;
using System.Collections;
public class MultiplayerLevelController : MonoBehaviourPunCallbacks
{   

     public static  MultiplayerLevelController instance;
    [SerializeField] private CinemachineCamera freeLookCamera;
    public List<Transform> spawnpostions;

      Health Localhealth;

   // public static event Action Onkillcountchange;

    MPUIcontrol mpui;
     
     GameObject localplayer;

    public override void OnEnable()
    {
        base.OnEnable();
         Health.OnMPplayerdead += HandlePlayerKilled;
         mpui=GetComponent<MPUIcontrol>();

         instance=this;
         
    }

    public override void OnDisable()
    {
        base.OnDisable();
        Health.OnMPplayerdead -= HandlePlayerKilled;
        
    }

   
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 2)
            return;

        if (!PhotonNetwork.InRoom)
            return;

            SpawnPlayer();
        Initializelocalplayerstats();

    }

    void SpawnPlayer()
    {
        if (!PhotonNetwork.InRoom)return;

        int spawnPos = Random.Range(0,spawnpostions.Count);

         localplayer = PhotonNetwork.Instantiate("Player", spawnpostions[spawnPos].position, Quaternion.identity);

        // Camera setup ONLY for local player
        PlayerCameraSetup camSetup =
            localplayer.GetComponent<PlayerCameraSetup>();

        if (camSetup != null)
        {
            camSetup.SetupCamera(freeLookCamera);
        }

        Localhealth=localplayer.GetComponent<Health>();

    }

     void Initializelocalplayerstats()
    {
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable

        {
            {PlayerProterties.Kills,0},
            {PlayerProterties.Deaths,0}
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        Debug.Log("initialized player stats");
    }

    
     void HandlePlayerKilled(Player killer, Player victim)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        IncrementPlayerStat(victim, PlayerProterties.Deaths);

        if (killer != null && killer != victim)
        {
            IncrementPlayerStat(killer, PlayerProterties.Kills);
        }
    }

   void IncrementPlayerStat(Player player, string key)
   {
     if (!PhotonNetwork.IsMasterClient || player == null)
        return;

    int currentValue = 0;

    if (player.CustomProperties.ContainsKey(key))
        currentValue = (int)player.CustomProperties[key];

    ExitGames.Client.Photon.Hashtable props =
        new ExitGames.Client.Photon.Hashtable
        {
            { key, currentValue + 1 }
        };

    player.SetCustomProperties(props);

    Debug.Log($"[STAT] {player.NickName} {key} = {currentValue + 1}");
}


    IEnumerator Respawn()
    {
        
        yield return new  WaitForSecondsRealtime(3);

        int pos=Random.Range(0,spawnpostions.Count);

         localplayer.transform.position=spawnpostions[pos].position;
          
          localplayer.GetComponent<Playerinput>().enabled=true;
         // localplayer.GetComponent<CharacterController>().enabled=true;

          localplayer.GetComponent<Health>().OnRespawn();
          mpui.Hidedeadpanel();
    }

   public void StartRespawn()
    {
        mpui.Showdeadpanel();
        StartCoroutine(Respawn());
    }

    public void DisableLocalPlayer()
{
    if (localplayer == null)
        return;

    // Disable movement / shooting
    Playerinput input = localplayer.GetComponent<Playerinput>();
    if (input != null)
        input.enabled = false;

    // Disable CharacterController if you use it
     localplayer.GetComponent<PlayerMovement>().enabled=false;
   

   
}

   

}
     