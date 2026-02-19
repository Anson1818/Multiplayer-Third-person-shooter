using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     private CharacterController Controller;
     private Playerinput input;
     private Camera cam;

      public float Walkspeed=5f;
      public float Runspeed=7f;
      public float Rotationspeed=10f;
        
         public float gravity = -9.81f;
         public float jumpForce = 8f;
         private Vector3 velocity;
    void Awake()
    {
        Controller=GetComponent<CharacterController>();
        input=GetComponent<Playerinput>();
        cam=Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
          if(PhotonNetwork.InRoom && !photonView.IsMine)return;
        ApplyGravityAndJump();
        MovePlayer();
        RotateWithCamera();
    }

    void MovePlayer()
    {
        Vector2 move = input.Move;

        
        float speed = input.Sprint ? Runspeed : Walkspeed;

        
        Vector3 camForward = cam.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cam.transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        
        Vector3 direction = camForward * move.y + camRight * move.x;

        
        Controller.Move(direction * speed * Time.deltaTime);
    }

    void RotateWithCamera()
    {
        
        Vector3 camForward = cam.transform.forward;
        camForward.y = 0f;   
        camForward.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(camForward);

        
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Rotationspeed * Time.deltaTime
        );
    }

  void ApplyGravityAndJump()
    {
       
        if ( velocity.y < 0)
        {
            velocity.y = -2f;  
        }

        
         if (Controller.isGrounded && input.Jump)
        {
            velocity.y = jumpForce;
        }

       
        velocity.y += gravity * Time.deltaTime;

        
        Controller.Move(velocity * Time.deltaTime);
    }


}
