using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class WorldUIManager : MonoBehaviour
    {
        public static WorldUIManager Instance { get; private set; }

        public Image WaveTimerUI;
        public TextMeshProUGUI WaveTextHolder;

        public Image HeroUI;
        public TextMeshProUGUI HeroTextHolder;

        public Image SubHeroUI;
        public TextMeshProUGUI SubHeroTextHolder;

        public Image EnemyAmoutUI;
        public TextMeshProUGUI EnemyTextHolder;

        public TextMeshProUGUI WaveCountText;

        [SerializeField] private RectTransform infoPanel;
        [SerializeField] private TextMeshProUGUI infoText;

        [Header("Enemy Info")] 
        [SerializeField] private RectTransform enemyInfoField;
        [SerializeField] private TextMeshProUGUI enemyInfoText;

        [Header("Hero Info")] 
        [SerializeField] private RectTransform heroInfoField;
        [SerializeField] private TextMeshProUGUI heroInfoText;

        public List<Image> EnemyPictures;
        public List<Image> HeroPictures;

        [SerializeField] private RectTransform showScreenShotImage;

        private bool toggle;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void UpdateEnemyInfoText(string text)
        {
            enemyInfoField.gameObject.SetActive(false);
            StartCoroutine(NextEnemy());
            enemyInfoText.text = text;
        }
        
        public void UpdateHeroInfoText(string text)
        {
            heroInfoField.gameObject.SetActive(false);
            StartCoroutine(NextHero());
            heroInfoText.text = text;
        }

        private IEnumerator NextEnemy()
        {
            yield return new WaitForSeconds(0.3f);
            enemyInfoField.gameObject.SetActive(!false);
        }
        
        private IEnumerator NextHero()
        {
            yield return new WaitForSeconds(0.1f);
            heroInfoField.gameObject.SetActive(!false);
        }

        public void ShowFireWorkUI()
        {
            StopAllCoroutines();
            infoPanel.gameObject.SetActive(true);
            AnimationManager.Instance.FireworkItemClick();
            infoText.text = "Firework : Destroy all enemies in the field.\n(can only be used once) ";
            StartCoroutine(FireWorkClicked());
            StartCoroutine(DisableInfo());
        }

        public void ShowCookieUI()
        {
            Debug.Log("CheckChgeck");
            StopAllCoroutines();
            infoPanel.gameObject.SetActive(true);
            AnimationManager.Instance.CookieItemClick();
            infoText.text = "Cookie : Increases all character's ATK Damage 30% for 30s. (can only be used once) ";
            StartCoroutine(CookieClicked());
            StartCoroutine(DisableInfo());
        }

        public void ShowCandy()
        {
            StopAllCoroutines();
            infoPanel.gameObject.SetActive(true);
            AnimationManager.Instance.CandyItemClick();
            infoText.text = "Candy : Increases all character's ATK Speed 30% for 30s. (can only be used once) ";
            StartCoroutine(CandyClicked());
            StartCoroutine(DisableInfo());
        }

        public void ShowScreenShot()
        {
            AnimationManager.Instance.ScreenShotClick();
            StartCoroutine(ScreenShotClicked());
        }

        public void ShowLeaderBoard()
        {
            AnimationManager.Instance.LeaderBoardClick();
            StartCoroutine(LeaderBoardClicked());
        }

        private IEnumerator DisableInfo()
        {
            yield return new WaitForSeconds(4f);
            infoPanel.gameObject.SetActive(false);
        }

        private IEnumerator FireWorkClicked()
        {
            yield return new WaitForSeconds(0.05f);
            AnimationManager.Instance.FireworkItemClicked();
        }

        private IEnumerator CookieClicked()
        {
            yield return new WaitForSeconds(0.05f);
            AnimationManager.Instance.CookieItemClicked();
        }

        private IEnumerator CandyClicked()
        {
            yield return new WaitForSeconds(0.05f);
            AnimationManager.Instance.CandyItemClicked();
        }

        private IEnumerator ScreenShotClicked()
        {
            yield return new WaitForSeconds(0.05f);
            AnimationManager.Instance.ScreenShotClicked();
        }

        private IEnumerator LeaderBoardClicked()
        {
            yield return new WaitForSeconds(0.05f);
            AnimationManager.Instance.LeaderBoardClicked();
        }

        public void OnScreenShotToggle()
        {
            toggle = !toggle;
            if (toggle)
            {
                showScreenShotImage.gameObject.SetActive(true);
            }
            else
            {
                showScreenShotImage.gameObject.SetActive(false);
            }
        }
    }
}