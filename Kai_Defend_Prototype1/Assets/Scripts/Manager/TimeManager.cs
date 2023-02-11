using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public bool IsPause;
    public bool DisableControler;
    
    [Range(0, 3)] [SerializeField] private float PausetTimeScale;
    [Range(0, 1)] [SerializeField] private float defaultTimeScale;
    // Start is called before the first frame update

    public static TimeManager Instance { get; private set; }

    private void Awake()
    {
        Debug.Assert(PausetTimeScale == 0, "timeScale is not zero");
        Debug.Assert(defaultTimeScale != 0, "defaultTimeScale is zero");

        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        GameManager.Instance.GameOver += GameOver;
        // AdsManager.Instance.OnAdsReward += AdsReward;
    }

    private void Update()
    {
        if (IsPause)
        {
            Time.timeScale = PausetTimeScale;
        }
        else
        {
            Time.timeScale = defaultTimeScale;
        }
    }


    // private void Update()
    // {
    //     //Check Is Player In game?
    //     if (UiManager.Instance.IsGameStart)
    //     {
    //         if (Input.GetKeyUp(KeyCode.Escape))
    //         {
    //             SoundManager.Instance.Play(SoundManager.Sound.ButtonClicked);
    //
    //             //Toggle button
    //             toggle = !toggle;
    //             //Pause && Unpause BGM
    //             SoundManager.Instance.Pause(SoundManager.Sound.MotorLoop, toggle);
    //             SoundManager.Instance.Pause(SoundManager.Sound.InGameBGM, toggle);
    //             SoundManager.Instance.Pause(SoundManager.Sound.Break, toggle);
    //             SoundManager.Instance.Pause(SoundManager.Sound.CarDrift, toggle);
    //             Time.timeScale = toggle ? timeScale : defaultTimeScale;
    //
    //             if (toggle)
    //             {
    //                 UiManager.Instance.ShowInGameMenu();
    //             }
    //             else
    //             {
    //                 UiManager.Instance.HideInGameMenu();
    //             }
    //         }
    //     }
    // }

    public void Pause()
    {
        IsPause = true;
        DisableControler = true;
    }
    
    public void Resume()
    {
        IsPause = false;
        DisableControler = false;
    }
    
    private void GameOver()
    {
        StartCoroutine(WaitForPanel());
    }

    private IEnumerator WaitForPanel()
    {
        yield return new WaitForSeconds(1);
        IsPause = true;
    }
    
    private IEnumerator WaitForUnPause()
    {
        yield return new WaitForSeconds(1);
        IsPause = false;
    }
    public void AdsReward()
    {
        StopAllCoroutines();
        StartCoroutine(WaitForUnPause());
    }



    // public void UnPauseGame()
    // {
    //     //Toggle if other input call to unPause
    //     toggle = !toggle;
    //     SoundManager.Instance.Pause(SoundManager.Sound.InGameBGM, toggle);
    //     SoundManager.Instance.Pause(SoundManager.Sound.MotorLoop, toggle);
    //     SoundManager.Instance.Pause(SoundManager.Sound.Break, toggle);
    //     SoundManager.Instance.Pause(SoundManager.Sound.CarDrift, toggle);
    //     Time.timeScale = defaultTimeScale;
    // }
}