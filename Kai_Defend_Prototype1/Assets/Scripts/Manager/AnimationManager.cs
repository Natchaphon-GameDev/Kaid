using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instance { get; private set; }
    
    [Header("Map")] 
    [SerializeField] private Animator mapField;
    
    [Header("3D Button")]
    [SerializeField] private Animator itemFirework;
    [SerializeField] private Animator itemCookie;
    [SerializeField] private Animator itemCandy;
    [SerializeField] private Animator screenShot;
    [SerializeField] private Animator leaderBoard;
    
    [Header("Notice")]
    [SerializeField] private Animator noticeTimer;
    [SerializeField] private Animator notice;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void MapUp()
    {
        mapField.SetBool("IsHeroPlace",true);
    }
    
    public void MapDown()
    {
        mapField.SetBool("IsHeroPlace",false);
    }

    public void FireworkItemClick()
    {
        itemFirework.SetBool("ItemClick",true);
    }

    public void FireworkItemClicked()
    {
        itemFirework.SetBool("ItemClick",false);
    }
    
    public void CookieItemClick()
    {
        itemCookie.SetBool("ItemClick",true);
    }
    
    public void CookieItemClicked()
    {
        itemCookie.SetBool("ItemClick",false);
    }
    
    public void CandyItemClick()
    {
        itemCandy.SetBool("ItemClick",true);
    }
    
    public void CandyItemClicked()
    {
        itemCandy.SetBool("ItemClick",false);
    }
    
    public void ScreenShotClick()
    {
        screenShot.SetBool("ItemClick",true);
    }
    
    public void ScreenShotClicked()
    {
        screenShot.SetBool("ItemClick",false);
    }
    
    public void LeaderBoardClick()
    {
        leaderBoard.SetBool("ItemClick",true);
    }
    
    public void LeaderBoardClicked()
    {
        leaderBoard.SetBool("ItemClick",false);
    }
    
    public void NoticeTimerIn()
    {
        noticeTimer.SetBool("Notice",true);
    }
    
    public void NoticeTimerOut()
    {
        noticeTimer.SetBool("Notice",false);
    }
    
    public void NoticeIn()
    {
        notice.SetBool("Notice",true);
    }
    
    public void NoticeOut()
    {
        notice.SetBool("Notice",false);
    }

}
