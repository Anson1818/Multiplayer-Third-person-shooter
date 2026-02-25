using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HomeScreen : MonoBehaviour
{
    [Header("Panels")]
    public GameObject levelPanel;
    public GameObject weaponPanel;
    public GameObject lobbyPanel;
    public GameObject loadingpanel;

    public static event Action OnESCPressed;

    private InputSystem_Actions input;

     void OnEnable()
    {
        input= new InputSystem_Actions();
        input.Enable();
        Launcher.Onjoinlobby += ShowLobby;
        input.Player.Pause.performed+=PressedESC;
        
    }

    void OnDisable()
    {
        Launcher.Onjoinlobby -= ShowLobby;
        input.Disable();
    }

    // ---------- UI Navigation ----------

    public void ShowLevelPanel()
    {
        MenuManager.Instance.OpenPanel(levelPanel);
    }

    public void ShowWeaponPanel()
    {
        MenuManager.Instance.OpenPanel(weaponPanel);
    }

    public void ShowLobby()
    {
        MenuManager.Instance.OpenPanel(lobbyPanel);
        loadingpanel.SetActive(false);
    }

    // ---------- Game Selection ----------

    public void SelectLevel(int index)
    {
        GameData.selectedLevel = index;
    }

    public void SelectWeapon(int index)
    {
        GameData.selectedWeapon = index;
    }

    // ---------- Scene Loading ----------

    public void LoadLevel()
    {
        SceneManager.LoadScene(GameData.selectedLevel);
        Time.timeScale = 1f;
    }

    // ---------- Quit ----------

    public void CloseApplication()
    {
        Application.Quit();
    }

     void PressedESC(InputAction.CallbackContext ctx)
    {
       OnESCPressed?.Invoke();   
    }

    public void ShowLoadingscreen()
    {
        loadingpanel.SetActive(true);
    }
}