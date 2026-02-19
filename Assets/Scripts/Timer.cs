using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float startTime = 120f; 

    public TextMeshProUGUI timerText;

    private float currentTime;

    void Start()
    {
        // Initialize timer
        currentTime = startTime;
    }

    void Update()
    {
        // Decrease time
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            currentTime = 0; // clamp at zero
        }

        // Convert to minutes:seconds
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        // Display
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}