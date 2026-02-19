using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreen : MonoBehaviour
{
    public GameObject levelPanel;
    public GameObject weaponPanel;
    public GameObject Lobbypanel;

    void Awake()
    {
        Launcher.Onjoinlobby+=ShowLoby;
    }
    void OnDisable()
    {
        Launcher.Onjoinlobby-=ShowLoby;
    }

    public void ShowLevelPanel() 
    { 
        levelPanel.SetActive(true);
    }
    public void ShowWeaponPanel()
    {
         weaponPanel.SetActive(true);
    }

    
    public void HideLevelPanel() 
    {
         levelPanel.SetActive(false);
    }
    public void HideWeaponPanel()
    { 
        weaponPanel.SetActive(false);
    }

  
    public void SelectLevel(int index)
    {
        GameData.selectedLevel = index;
        HideLevelPanel();
    }

    
    public void SelectWeapon(int index)
    {
        GameData.selectedWeapon = index;
        HideWeaponPanel();
    }

   
    public void LoadLevel()
    {
        SceneManager.LoadScene(GameData.selectedLevel);
        Time.timeScale=1f;
    }

    public void ShowLoby()
    {
        Lobbypanel.SetActive(true);
    }
    public void Hidelobbypanel()
    {
        Lobbypanel.SetActive(false);
    }

    public void Closeapplication()
    {
        Application.Quit();
    }
    
}
