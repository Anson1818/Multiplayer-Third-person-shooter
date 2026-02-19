using UnityEngine;
using UnityEngine.AI;

public class BotAI : MonoBehaviour
{
   public Transform player;
   public float detectionRange=15f;
   public float shootingRange=8f;

   private NavMeshAgent agent;
   private BotShooting botshoot;
    public float fireRate = 0.5f;
    private float nextFireTime;
   private Animator botanimation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // void Awake()
    // {
    //     player=GameObject.FindGameObjectWithTag("Player").transform;
    // }
    void Start()
    {
         agent=GetComponent<NavMeshAgent>();
         botshoot=GetComponent<BotShooting>();
         botanimation=GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {

         float distance=Vector3.Distance(transform.position,player.position);

          
          if(distance>detectionRange)
        {
            Idle();
        }
        else if(distance>shootingRange)
        {
            Chase();
        }
        else
        {
            Shoot();
        }
        
    }

     void Idle()
    {
        agent.isStopped=true;
        botshoot.StopShooting();
        botanimation.SetFloat("MoveY",0);
    }
    void Chase()
    {
        botshoot.StopShooting();
        agent.isStopped = false;
        agent.SetDestination(player.position);
        botanimation.SetFloat("MoveY",1);
    }

    void Shoot()
    {
        agent.isStopped = true;

       
        Vector3 dir = (player.position - transform.position);
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir);

         botanimation.SetBool("Fire",true);
         botanimation.SetFloat("MoveY",0);
         
         botshoot.allowshooting();
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            botshoot.Fire();
        }
    }
   
}
