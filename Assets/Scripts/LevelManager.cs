using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
     public static LevelManager instance;
     public static event Action<int> Sendkilldata;
     public static event Action Onplayerwin;
     public static event Action Onplayerlose;
     public static event Action Onhalftime;

     public int killcount=0;
     public float timer=120f;

     private bool gameended=false;

    void Awake()
    {
        if(instance==null)
        {
            instance=this;
        }
        else
        {
            Destroy(gameObject);
        }

        Health.Onbotdead+=Updatekillcount;
    }
    void OnDisable()
    {
        Health.Onbotdead-=Updatekillcount;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameended)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = 0;
            Wingame();
        }

        if(timer==60)
        {
            Onhalftime?.Invoke();
        }
    }
    public void Updatekillcount()
    {
        killcount++;
       Sendkilldata?.Invoke(killcount);

    }

    public void Losegame()
    {
       gameended=true;
       Onplayerlose?.Invoke();
    }
     void Wingame()
    {
        gameended=true;
        Onplayerwin?.Invoke();
    }
}
