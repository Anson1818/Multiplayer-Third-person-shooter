using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
     public GameObject botPrefab;
     public Transform[] spawnPoints;

     public float spawnInterval=3f;
     public int maxBots=5;
     private float nextSpawnTime;
    


    
    // Update is called once per frame
     void Update()
    {
        // Only spawn when below max bots AND cooldown is over
        if (Time.time >= nextSpawnTime && GetAliveBotCount() < maxBots)
        {
            SpawnBot();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnBot()
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject newBot = Instantiate(botPrefab, point.position, point.rotation);

        // FIXED: assign player to the NEW bot
        BotAI ai = newBot.GetComponent<BotAI>();
        ai.player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    int GetAliveBotCount()
    {
        // Count only ACTIVE bots in scene
        return GameObject.FindGameObjectsWithTag("Bot").Length;
    }

}
