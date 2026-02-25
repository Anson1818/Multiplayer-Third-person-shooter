using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;


public class MenuManager : MonoBehaviourPunCallbacks
{
    public static MenuManager Instance;

    private Stack<GameObject> panelStack = new Stack<GameObject>();

    [Header("Main Panels (Stack Controlled)")]
    public GameObject landingPanel;
    public GameObject lobbyPanel;

    [Header("UI")]
    public GameObject backButton;
    public GameObject quitPopupPanel;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
public override void OnEnable()
{
    base.OnEnable();
    HomeScreen.OnESCPressed += HandleEscape;
}

 public override void OnDisable()
{
    base.OnDisable();
    HomeScreen.OnESCPressed -= HandleEscape;
}

    void Start()
    {
        OpenPanel(landingPanel);
        quitPopupPanel.SetActive(false);
        UpdateBackButton();
    }

    // =========================
    // ESC Handling
    // =========================

    public void HandleEscape()
    {
        // Close quit popup first
        if (quitPopupPanel.activeSelf)
        {
            quitPopupPanel.SetActive(false);
            return;
        }

        if (panelStack.Count > 1)
        {
            GoBack();
        }
        else
        {
            quitPopupPanel.SetActive(true);
        }
    }

    // =========================
    // Stack Navigation
    // =========================

    public void OpenPanel(GameObject panel)
    {
        if (panelStack.Count > 0)
            panelStack.Peek().SetActive(false);

        panel.SetActive(true);
        panelStack.Push(panel);

        UpdateBackButton();
    }

    public void GoBack()
    {
        if (panelStack.Count <= 1)
            return;

        GameObject currentPanel = panelStack.Peek();

        // If leaving Lobby → handle Photon correctly
        if (currentPanel == lobbyPanel)
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
                
            }

            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveLobby();
                PhotonNetwork.Disconnect();
                
            }
        }

        PerformPanelPop();
    }

    void PerformPanelPop()
    {
        if (panelStack.Count <= 1)
            return;

        GameObject top = panelStack.Pop();
        top.SetActive(false);

        panelStack.Peek().SetActive(true);

        UpdateBackButton();
    }

    void UpdateBackButton()
    {
        backButton.SetActive(panelStack.Count > 1);
    }

    // =========================
    // Photon Callbacks
    // =========================

    public override void OnLeftRoom()
    {
        // After leaving room, also leave lobby if needed
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        else
        {
            PerformPanelPop();
        }
    }

    public override void OnLeftLobby()
    {
        PerformPanelPop();
    }

    // =========================
    // Quit
    // =========================

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CloseQuitPopup()
    {
        quitPopupPanel.SetActive(false);
    }

    
}