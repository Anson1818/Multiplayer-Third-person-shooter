
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UI : MonoBehaviour
{

      public Image Healthbar;
      public TextMeshProUGUI Ammoncount;
      public Image gunicon;

      public TextMeshProUGUI Killcount;
     public GameObject winpanel;
     public GameObject losepanel;
     public GameObject Pausepanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        Health.OnHealthchange+=Healthvalue;
        PlayerShooting.Onammochanged+=updateammocount;
        WeaponManager.OnEquip+=Gunimage;
        LevelManager.Sendkilldata+=UpdateKillcount;
        LevelManager.Onplayerwin+=Winscreen;
        Health.Onplayerdead+=Losescreen;
        Playerinput.Onpausepressed+=Pausescreen;

    }

    void OnDisable()
    {
        Health.OnHealthchange-=Healthvalue;
        PlayerShooting.Onammochanged-=updateammocount;
        WeaponManager.OnEquip-=Gunimage;
        LevelManager.Sendkilldata-=UpdateKillcount;
        LevelManager.Onplayerwin-=Winscreen;
        Health.Onplayerdead-=Losescreen;
        Playerinput.Onpausepressed-=Pausescreen;
    }

    void Healthvalue(float value)
    { 
         float fillnumber=value/100f;
        Healthbar.fillAmount=fillnumber;
    }

    void updateammocount(int count)
    {
        Ammoncount.text=count.ToString();
    }

    void Gunimage(Sprite icon)
    {
        gunicon.sprite=icon;
    }

    void UpdateKillcount(int count)
    {
        Killcount.text=count.ToString();
    }

    void Losescreen()
    {
        losepanel.SetActive(true);
        Time.timeScale=0f;
    }
    void Winscreen()
    {
        winpanel.SetActive(true);
        Time.timeScale=0f;
    }
    public void Loadhomescreen()
    {
         if(PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }
        SceneManager.LoadScene(0);
    }
    public void Loadcurrentscene()
    {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
         Time.timeScale=1f;
    }

    public void Pausescreen()
    {
         Pausepanel.SetActive(true);
         Time.timeScale=0f;
    }
    public void Resume()
    {
        Pausepanel.SetActive(false);
        Time.timeScale=1f;
    }

    
}
