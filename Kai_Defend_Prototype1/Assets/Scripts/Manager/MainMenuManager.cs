using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance { get; private set; }
    
    private bool toggleUpload;

    public RectTransform UploadPanel;
    public RectTransform CreditPanel;
    public RectTransform OptionPanel;
    public RectTransform HowToPlayPanel;
    public RectTransform LeaderboardPanel;
    public RectTransform PlayerDataPanel;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenUploadPhoto()
    {
        toggleUpload = !toggleUpload;
        if (toggleUpload)
        {
            UploadPanel.gameObject.SetActive(true);
        }
        else
        {
            UploadPanel.gameObject.SetActive(!true);
        }
    }

    public void OpenLeaderBoard()
    {
        UploadPanel.gameObject.SetActive(!true);
        CreditPanel.gameObject.SetActive(!true);
        OptionPanel.gameObject.SetActive(!true);
        HowToPlayPanel.gameObject.SetActive(!true);
        LeaderboardPanel.gameObject.SetActive(true);
        PlayerDataPanel.gameObject.SetActive(!true);
    }

    public void OpenCredit()
    {
        UploadPanel.gameObject.SetActive(!true);
        CreditPanel.gameObject.SetActive(true);
        OptionPanel.gameObject.SetActive(!true);
        HowToPlayPanel.gameObject.SetActive(!true);
        LeaderboardPanel.gameObject.SetActive(!true);
        PlayerDataPanel.gameObject.SetActive(!true);
    }
    
    public void OpenHowToPlay()
    {
        UploadPanel.gameObject.SetActive(!true);
        CreditPanel.gameObject.SetActive(!true);
        OptionPanel.gameObject.SetActive(!true);
        HowToPlayPanel.gameObject.SetActive(true);
        LeaderboardPanel.gameObject.SetActive(!true);
        PlayerDataPanel.gameObject.SetActive(!true);
    }
    
    public void OpenMenu()
    {
        UploadPanel.gameObject.SetActive(!true);
        CreditPanel.gameObject.SetActive(!true);
        OptionPanel.gameObject.SetActive(!true);
        HowToPlayPanel.gameObject.SetActive(!true);
        LeaderboardPanel.gameObject.SetActive(!true);
        PlayerDataPanel.gameObject.SetActive(true);
    }

    
    public void OpenOption()
    {
        UploadPanel.gameObject.SetActive(!true);
        CreditPanel.gameObject.SetActive(!true);
        OptionPanel.gameObject.SetActive(true);
        HowToPlayPanel.gameObject.SetActive(!true);
        LeaderboardPanel.gameObject.SetActive(!true);
        PlayerDataPanel.gameObject.SetActive(!true);
    }

    
}
