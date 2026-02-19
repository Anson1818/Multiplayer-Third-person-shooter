using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
     Animator anim;
    CharacterController controller;
    Playerinput input;

    void Awake()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        input = GetComponent<Playerinput>();
    }

    void Update()
    {
        anim.SetFloat("MoveX", input.Move.x);
         
        anim.SetFloat("MoveY", input.Move.y);

        anim.SetBool("Isrunning", input.Sprint&&controller.velocity.magnitude>0);

        
        anim.SetBool("jump", input.Jump&&controller.isGrounded);


       
    }
}
