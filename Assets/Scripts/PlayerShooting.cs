using System;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class PlayerShooting : MonoBehaviourPun
{
    
    public static event Action<int> Onammochanged;
     public  Weapons currentWeapon;
    public float fireRate = 0.15f;
   

     Playerinput input;
     Animator animator;

     Camera cam;

     public GameObject hiteffect;
     public GameObject hiteffectred;

     float nextFireTime = 0f;
    void Awake()
    {
        input=GetComponent<Playerinput>();
        cam=Camera.main;
        animator=GetComponent<Animator>();
    }

    void Start()
    {
      // Onammochanged?.Invoke(currentweapon.currentammo);
    }

    // Update is called once per frame
    void Update()
    {
        // Ownership check ONLY for multiplayer
        if (IsMultiplayer() && !photonView.IsMine)
            return;

        HandleFire();
        HandleReload();
    }

    // ============================
    // CORE LOGIC
    // ============================

    void HandleFire()
    {
        bool canShoot = input.Shoot && Time.time >= nextFireTime;

        // Stop fire animation when input stops
        animator.SetBool("Fire", canShoot);

        if (!canShoot)
            return;

        nextFireTime = Time.time + fireRate;

        if (currentWeapon.currentammo <= 0)
            return;

        currentWeapon.currentammo =
            Mathf.Max(0, currentWeapon.currentammo - 1);

        Onammochanged?.Invoke(currentWeapon.currentammo);

        if (IsMultiplayer())
            Shoot_Multiplayer();
        else
            Shoot_SinglePlayer();
    }

    void HandleReload()
    {
        if (!input.Reload)
            return;

        currentWeapon.currentammo = currentWeapon.Toatalammo;
        Onammochanged?.Invoke(currentWeapon.currentammo);
    }

    // ============================
    // SINGLE PLAYER SHOOT
    // ============================

    void Shoot_SinglePlayer()
    {
        PlayLocalEffects();

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, currentWeapon.range))
        {
            SpawnHitEffect(hit.point, hit.collider.CompareTag("Bot"));

            if (hit.collider.CompareTag("Bot"))
            {
                Health hp = hit.collider.GetComponent<Health>();
                if (hp != null)
                    hp.Damage(currentWeapon.damage);
            }
        }
    }

    // ============================
    // MULTIPLAYER SHOOT
    // ============================

    void Shoot_Multiplayer()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        Vector3 hitPoint = Vector3.zero;
        bool hitplayer = false;

        if (Physics.Raycast(ray, out hit, currentWeapon.range))
        {
            hitPoint = hit.point;
               
               PhotonView targetview=hit.collider.GetComponent<PhotonView>();


               if(targetview!=null&& !targetview.IsMine)
            {
                hitplayer=true;
            
                Health hp=hit.collider.GetComponent<Health>();
                if(hp!=null)
               {  
               hp.Damage(currentWeapon.damage,PhotonNetwork.LocalPlayer.ActorNumber);
               }
            }
            
        }
          PlayLocalEffects();
          SpawnHitEffect(hitPoint,hitplayer);
        // RPC → everyone plays effects
        photonView.RPC(
            nameof(RPC_Shoot),
            RpcTarget.Others,
            hitPoint,
            hitplayer
        );

    }

    // ============================
    // RPC
    // ============================

    [PunRPC]
    void RPC_Shoot(Vector3 hitPoint, bool hitplayer)
    {
       if (hitPoint == Vector3.zero)return;

           GameObject effect= hitplayer? hiteffectred:hiteffect;

           Instantiate(effect,hitPoint,quaternion.identity); 
            
          PlayLocalEffects();
    }

    // ============================
    // SHARED VISUALS
    // ============================

    void PlayLocalEffects()
    {
        currentWeapon.PlaySound();
        currentWeapon.PlayMuzzleFlash();
    }

    void SpawnHitEffect(Vector3 point, bool isBot)
    {
        GameObject effect =
            isBot ? hiteffectred : hiteffect;

        Instantiate(effect, point, quaternion.identity);
    }

    // ============================
    // HELPERS
    // ============================

    bool IsMultiplayer()
    {
        return PhotonNetwork.InRoom;
    }
}