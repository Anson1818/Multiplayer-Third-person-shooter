using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playerinput : MonoBehaviourPun
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   private InputSystem_Actions Inputsystem;

   public Vector2 Move  {get;private set;}
   public Vector2 Look  {get;private set;}
   public bool Jump {get;private set;}
   public bool Shoot {get;private set;}
   public bool Sprint {get;private set;}
   public bool Reload{get;private set;}
   public bool paused {get; private set;}

   public static event Action Onpausepressed;
    void Awake()
    {
        Inputsystem= new InputSystem_Actions();
    }

    void OnEnable()
    {
        Inputsystem.Enable();
    }

    void OnDisable()
    {
        Inputsystem.Disable();
    }

    // Update is called once pper frame
    void Update()
    {
          if(PhotonNetwork.InRoom && !photonView.IsMine)return;
         
          Move=Inputsystem.Player.Move.ReadValue<Vector2>();
          Look=Inputsystem.Player.Look.ReadValue<Vector2>();
          Jump=Inputsystem.Player.Jump.IsPressed();
          Shoot=Inputsystem.Player.Attack.IsPressed();
          Sprint=Inputsystem.Player.Sprint.IsPressed();
          Reload=Inputsystem.Player.Reload.WasPressedThisFrame();
          paused=Inputsystem.Player.Pause.WasPressedThisFrame();

       if(paused)
        {
             Onpausepressed?.Invoke();
        }

    }
}
