using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Health : MonoBehaviourPun
{
    public static event Action<float> OnHealthchange;

    public float Maxhealth = 100f;
    private float Currenthealth;

    public bool bot;
    private bool isdead = false;

    public static event Action Onbotdead;
    public static event Action Onplayerdead;
  

    public static event Action<Player,Player> OnMPplayerdead;

    // 🔑 Multiplayer: track last attacker
    private int lastAttackerActorNumber = -1;

    void OnEnable()
    {
        LevelManager.Onhalftime+=Healthboost;
    }
    void OnDisable()
    {
        LevelManager.Onhalftime-=Healthboost;
    }



    void Start()
    {
        Currenthealth = Maxhealth;

        // Update health UI for local player (SP or MP)
        if (!bot && (!PhotonNetwork.InRoom || photonView.IsMine))
        {
            OnHealthchange?.Invoke(Currenthealth);
        }
    }

    void Healthboost()
{
    Currenthealth = Mathf.Min(Currenthealth + 30, Maxhealth);

    if (!bot && (!PhotonNetwork.InRoom || photonView.IsMine))
    {
        OnHealthchange?.Invoke(Currenthealth);
    }
}


  public void OnRespawn()
{
    // ONLY visual + local flags
    isdead = false;
    GetComponent<PlayerMovement>().enabled=true;
    GetComponent<Playerinput>().enabled=true;
    Animator anim = GetComponent<Animator>();
    if (anim != null)
    {
        anim.ResetTrigger("Die");
        anim.Play("Blend Tree", 0, 0f);
    }

    OnHealthchange?.Invoke(Maxhealth);

    // Ask Master to reset health properly
    photonView.RPC(nameof(RPC_RequestRespawn), RpcTarget.MasterClient);
}

 [PunRPC]
void RPC_RequestRespawn()
{
    if (!PhotonNetwork.IsMasterClient)
        return;

    // Master decides the real health value
    photonView.RPC(nameof(RPC_ApplyRespawn), RpcTarget.All);
}
   

   [PunRPC]
void RPC_ApplyRespawn()
{
    isdead = false;
    Currenthealth = Maxhealth;

    Animator anim = GetComponent<Animator>();
    if (anim != null)
    {
        anim.ResetTrigger("Die");
        anim.Play("Blend Tree", 0, 0f);
    }

    if (!bot && (!PhotonNetwork.InRoom || photonView.IsMine))
    {
        OnHealthchange?.Invoke(Currenthealth);
    }
}





    // =================================================
    // PUBLIC DAMAGE ENTRY POINT (SP + MP SAFE)
    // =================================================
    public void Damage(float amount, int attackerActorNumber = -1)
    {
        if (isdead)
            return;

        // SINGLE PLAYER
        if (!PhotonNetwork.InRoom)
        {
            ApplyDamage(amount);
            lastAttackerActorNumber=-1;
            return;
        }

           // lastAttackerActorNumber = attackerActorNumber;
        // MULTIPLAYER → request MasterClient
        if (PhotonNetwork.InRoom && attackerActorNumber <= 0)
        {
        Debug.LogError(  "[Health] Multiplayer Damage called without valid attackerActorNumber",this );
        return;
        }

        photonView.RPC(
            nameof(RPC_RequestDamage),
            RpcTarget.MasterClient,
            amount,
            attackerActorNumber
        );
    }

    // =================================================
    // MASTERCLIENT AUTHORITY
    // =================================================
    [PunRPC]
    void RPC_RequestDamage(float amount, int attackerActorNumber)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        photonView.RPC(
            nameof(RPC_ApplyDamage),
            RpcTarget.All,
            amount,
            attackerActorNumber
        );
    }

    // =================================================
    // APPLY DAMAGE ON ALL CLIENTS
    // =================================================
    [PunRPC]
    void RPC_ApplyDamage(float amount, int attackerActorNumber)
    {
        lastAttackerActorNumber = attackerActorNumber;
         Debug.Log(
        $"[DAMAGE] Victim={photonView.Owner.NickName} AttackerActor={attackerActorNumber}"
    );
        ApplyDamage(amount);
    }

    // =================================================
    // CORE DAMAGE LOGIC (SHARED)
    // =================================================
    void ApplyDamage(float amount)
    {
        if (isdead)
            return;

        Currenthealth -= amount;
            Debug.Log("getting damage");

        // Update UI only for local player
        if (!bot && (!PhotonNetwork.InRoom || photonView.IsMine))
        {
            OnHealthchange?.Invoke(Currenthealth);
        }

        if (Currenthealth <= 0)
        {
            Die();
        }
    }

    // =================================================
    // DEATH
    // =================================================
    void Die()
    {
        if (isdead)
            return;


        isdead = true;

        Animator anim = GetComponent<Animator>();
        if (anim != null)
            anim.SetTrigger("Die");

        // ---------------- BOT ----------------
        if (bot)
        {
            GetComponent<BotAI>().enabled = false;
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;

            Onbotdead?.Invoke();
            Destroy(gameObject, 2f);
            Healthboost();
            return;
        }

        // ---------------- SINGLE PLAYER ----------------
        if (!PhotonNetwork.InRoom)
        {
            Onplayerdead?.Invoke();
            return;
        }

        // ---------------- MULTIPLAYER (LOCAL PLAYER ONLY) ----------------
        if (photonView.IsMine&& lastAttackerActorNumber > 0)
        {  
            
            photonView.RPC(
                nameof(RPC_NotifyDeath),
                RpcTarget.MasterClient,
                lastAttackerActorNumber,
                PhotonNetwork.LocalPlayer.ActorNumber
            );

            

           // Onplayerdead?.Invoke();
        }

        GetComponent<PlayerMovement>().enabled=false;
        GetComponent<Playerinput>().enabled=false;

        if(photonView.IsMine)
        {          
          MultiplayerLevelController.instance.StartRespawn();
        }
        
    }

    
      [PunRPC]
void RPC_NotifyDeath(int killerActorNumber, int victimActorNumber)
{
    if (!PhotonNetwork.IsMasterClient)
        return;

    Player victim = PhotonNetwork.CurrentRoom.GetPlayer(victimActorNumber);
    Player killer = PhotonNetwork.CurrentRoom.GetPlayer(killerActorNumber);

   
        OnMPplayerdead?.Invoke(killer, victim);
}

}


