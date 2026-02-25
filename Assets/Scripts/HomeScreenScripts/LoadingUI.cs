using TMPro;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    public TextMeshProUGUI LoadingText;
      float pulseSpeed = 1.2f;
     float minAlpha = 0.3f;
     float maxAlpha = 1f;

    void Update()
    {
         float t = Mathf.PingPong(Time.time * pulseSpeed, 1f);
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, t);

        Color c = LoadingText.color;
        c.a = alpha;
        LoadingText.color = c;
    }
}
