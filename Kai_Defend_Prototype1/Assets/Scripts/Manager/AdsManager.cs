using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using Random = System.Random;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    private string gameId = "4416261";
    private bool testMode = true;

    // public event Action OnAdsReward;

    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);
        ShowBannerAd();
        RandomScrollManager.Instance.OnFirstRandomCard += HideBanner;
    }

    private void ShowBannerAd()
    {
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show("Banner_Android");
    }

    private void HideBanner()
    {
        Advertisement.Banner.Hide();
    }

    public void ShowAds(string placement)
    {
        Advertisement.Show(placement);
    }

    public void ShowMenuAds()
    {
        var rand = UnityEngine.Random.Range(1, 3);
        if (rand == 1)
        {
            ShowAds("Interstitial_Android");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidError(string message)
    {
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        Debug.Log($"This {placementId} Finished");
        
        if (showResult == ShowResult.Finished)
        {
            Debug.Log("Reward Here!");
            // OnAdsReward?.Invoke();
            //TODO : Refactor this idiot code
            //Dont know why it'll null ref when the scene was re-loaded
            UiManager.Instance.FinishAdsReward();
            GameManager.Instance.OnAdsReward();
            TimeManager.Instance.AdsReward();
            ItemManager.Instance.FireWork();
            CinemachineSwitcher.Instance.AdsReward();
            TimeManager.Instance.Resume();
            UiManager.Instance.AdsRewardButton.interactable = false;
        }
        else if(showResult == ShowResult.Failed)
        {
            Debug.Log("Not Reward");
        }
    }
}
