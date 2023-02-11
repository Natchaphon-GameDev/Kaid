using System;
using System.Collections;
using System.Collections.Generic;
using Map;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

namespace Manager
{
    public class UiManager : MonoBehaviour
    {
        public static UiManager Instance { get; private set; }

        public event Action OnButtonClicked;
        
        [Header("RandomScroll")]
        public RectTransform RandomScrollButtonArea;
        [SerializeField] private Button reRandomButton;
        [SerializeField] private Button closeRandomScrollButton;
        
        [Header("Money")] 
        [SerializeField] private TextMeshProUGUI money;

        [Header("GameOver")]
        [SerializeField] private TextMeshProUGUI timeCountBeforeOver;
        [SerializeField] private GameObject gameOver;
        
        [Header("Notification")]
        [SerializeField] private TextMeshProUGUI textNotification;

        [Header("Chain Skill")]
        [SerializeField] private RectTransform skillChainArea;
        [SerializeField] private TextMeshProUGUI skillChainText;
        [SerializeField] private List<TextMeshProUGUI> skillChainsList;
        
        [Header("Chain Element")]
        [SerializeField] private RectTransform elementChainArea;
        [SerializeField] private List<Image> elementChainsImg;
        public List<Image> FireElementList;
        public List<Image> WaterElementList;
        public List<Image> PlantElementList;

        [Header("BlockCanPlaceUI")]
        public Canvas blockUI;

        [Header("CardAmount")] 
        [SerializeField] private TextMeshProUGUI cardAmountText;
        public int cardAmount;
        
        [Header("ScoreEndGame")]
        [SerializeField] private TextMeshProUGUI timeSurvived;
        [SerializeField] private TextMeshProUGUI waveSurvived;
        [SerializeField] private TextMeshProUGUI damageDone;
        [SerializeField] private TextMeshProUGUI compUsed;

        [Header("RewardButton")]
        public Button AdsRewardButton;

        private int getMaxCardAmount;
        
        [HideInInspector]
        public float DamageDoneGameOver;


        [HideInInspector]
        public bool IsRandomPlus;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            reRandomButton.onClick.AddListener(OnReRandomButtonClicked);
            closeRandomScrollButton.onClick.AddListener(OnCloseRandomButtonClicked);
            
            // inventory.CardsInventory
            blockUI.gameObject.SetActive(false);
            timeCountBeforeOver.gameObject.SetActive(false);
            gameOver.gameObject.SetActive(false);
        }
        

        private void Start()
        {
            DamageDoneGameOver = 0;
            closeRandomScrollButton.gameObject.SetActive(!true);
            reRandomButton.gameObject.SetActive(!true);
            getMaxCardAmount = InventoryManager.Instance.CardsInventory.Count;
            GameManager.Instance.GameOver += SetDamageDone;
            RandomScrollManager.Instance.OnFirstRandomCard += ShowRandomButton;
            GameManager.Instance.GameOver += ShowGameOver;
        }

        public void FinishAdsReward()
        {
            gameOver.gameObject.SetActive(!true);
        }

        private void ShowRandomButton()
        {
            closeRandomScrollButton.gameObject.SetActive(true);
            reRandomButton.gameObject.SetActive(true);
        }

        public void OnRestartButtonClicked()
        {
            OnButtonClicked?.Invoke();
            SceneManager.LoadScene("Kai_Defend");
        }

        private void Update()
        {
            cardAmountText.text = $"Cards Inventory \n {cardAmount}/{getMaxCardAmount}";
            money.text = $"Money : {MoneyManager.Instance.Money} $";
            GetCurrentEnemyAmount();
            GetHeroSubHeroAmount();
        }

        public void UpdateCardInventory()
        {
            cardAmount = 0;
            foreach (var card in InventoryManager.Instance.CardsInventory)
            {
                if (card != null)
                {
                    cardAmount++;
                }
            }
        }

        private void ShowGameOver()
        {
            gameOver.gameObject.SetActive(true);
            AnimationManager.Instance.NoticeTimerOut();
            AnimationManager.Instance.NoticeOut();
        }

        public void SetWaveCount(string wave)
        {
            WorldUIManager.Instance.WaveCountText.text = wave;
        }

        private void GetHeroSubHeroAmount()
        {
            WorldUIManager.Instance.HeroUI.fillAmount = Area.CurrentHeroAmount / BuildManager.Instance.HeroLimit;
            WorldUIManager.Instance.SubHeroUI.fillAmount = Area.CurrentSubHeroAmount / BuildManager.Instance.SubHeroLimit;
            WorldUIManager.Instance.HeroTextHolder.text = $"{Area.CurrentHeroAmount} / {BuildManager.Instance.HeroLimit}";
            WorldUIManager.Instance.SubHeroTextHolder.text = $"{Area.CurrentSubHeroAmount} / {BuildManager.Instance.SubHeroLimit}";
        }

        private void GetCurrentEnemyAmount()
        {
            
            WorldUIManager.Instance.EnemyTextHolder.text =$"{GameManager.Instance.EnemyCount} / {GameManager.Instance.EnemyLimit}";
            WorldUIManager.Instance.EnemyAmoutUI.fillAmount = GameManager.Instance.EnemyCount / GameManager.Instance.EnemyLimit;
        }

        public void GetCountDownGameOver()
        {
            StopAllCoroutines();
            timeCountBeforeOver.gameObject.SetActive(true);
            timeCountBeforeOver.text = "Game Over In : " + GameManager.Instance.GetCountdownOverTime;
            AnimationManager.Instance.NoticeTimerIn();
        }
        
        public void HideTimerNotification()
        {
            StartCoroutine(WaitForNoticeTimerOut());
        }
        
        private IEnumerator WaitForNoticeTimerOut()
        {
            StartCoroutine(WaitForNoticeOut()); //Debug remain Noti 
            AnimationManager.Instance.NoticeTimerOut();
            yield return new WaitForSeconds(0.5f);
            timeCountBeforeOver.gameObject.SetActive(!true);
        }
        
        public void GetNotification()
        {
            StopAllCoroutines();
            textNotification.gameObject.SetActive(true);
            AnimationManager.Instance.NoticeIn();
            StartCoroutine(WaitForNoticeOut());
        }

        public void SetTextNotification(string text)
        {
            textNotification.text = text;
        }

        private IEnumerator WaitForNoticeOut()
        {
            yield return new WaitForSeconds(1.5f);
            AnimationManager.Instance.NoticeOut();
            yield return new WaitForSeconds(0.5f);
            textNotification.gameObject.SetActive(!true);
        }

        public void NotEnoughMoneyUI()
        {
            SetTextNotification("Not Enough Money");
            GetNotification();
        }
        
        public void OnBuyRandomScrollButtonClicked()
        {
            OnButtonClicked?.Invoke();
            if (RandomScrollManager.Instance.NotEnoughMoneyScore)
            {
                NotEnoughMoneyUI();
                return;
            }
            BuildManager.Instance.DeselectHero();
            RandomScrollManager.Instance.HideRandomScollButton.Invoke();
            IsRandomPlus = false;
            RandomScrollManager.Instance.ShowCard();
        }

        private void OnReRandomButtonClicked()
        {
            RandomScrollManager.Instance.ReRoll();
        }
        
        public void OnBuyRandomScrollPlusButtonClicked()
        {
            OnButtonClicked?.Invoke();
            if (RandomScrollManager.Instance.NotEnoughMoneyScorePlus)
            {
                NotEnoughMoneyUI();
                return;
            }
            BuildManager.Instance.DeselectHero();
            RandomScrollManager.Instance.HideRandomScollButton.Invoke();
            IsRandomPlus = true;
            RandomScrollManager.Instance.ShowCard();
        }
        
        public void OnCloseRandomButtonClicked()
        {
            OnButtonClicked?.Invoke();
            RandomScrollManager.Instance.ShowRandomScollButton.Invoke();
            RandomScrollManager.Instance.HideCard();
        }

        public void SpawnChainSkillText(string text)
        {
            skillChainText.text = $" - {text} -";
            skillChainsList.Add(Instantiate(skillChainText, skillChainArea));
        }
        
        public void RemoveChainSkillText(string text)
        {
            var temp = skillChainsList.Find(x => x.text == $" - {text} -");
            Destroy(temp.gameObject);
            skillChainsList.Remove(temp);
        }
        
        public void SpawnChainElement(int index)
        {
            if (index == 0)
            {
                FireElementList.Add(Instantiate(elementChainsImg[index], elementChainArea));
            }
            else if (index == 1)
            {
                WaterElementList.Add(Instantiate(elementChainsImg[index], elementChainArea));
            }
            else if (index == 2)
            {
                PlantElementList.Add(Instantiate(elementChainsImg[index], elementChainArea));
            }
        }
        
        public void RemoveChainElement(int index)
        {
            if (index == 0)
            {
                Destroy(FireElementList[FireElementList.Count - 1].gameObject);
                FireElementList.Remove(FireElementList[FireElementList.Count - 1]);
            }
            else if (index == 1)
            {
                Destroy(WaterElementList[WaterElementList.Count - 1].gameObject);
                WaterElementList.Remove(WaterElementList[WaterElementList.Count - 1]);
            }
            else if (index == 2)
            {
                Destroy(PlantElementList[PlantElementList.Count - 1].gameObject);
                PlantElementList.Remove(PlantElementList[PlantElementList.Count - 1]);
            }
        }

        //Show Score
        public void SetTimeSurvived(string text)
        {
            timeSurvived.text = text + " s";
        }
        
        public void SetWaveSurvived(string text)
        {
            waveSurvived.text = text + " Wave";
        }
        
        private void SetDamageDone()
        {
            damageDone.text = $"{(int)DamageDoneGameOver} Damage";
        }
        
        public void SetCompUsed(string text)
        {
            compUsed.text = text;
        }
    }
}