using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Map;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = System.Random;

namespace Manager
{
    public class InventoryManager : MonoBehaviour
    {
        [Header("Upgrade Hero Status")] 
        public float AtkDamage = 1;
        public float AtkSpeed = 0.05f;
        public float DmgOverTime = 1;
        
        [Header("Hero Manage")]
        public List<HeroBluePrint> HeroTeam;

        [Header("Inventory")]
        public List<CardBluePrint> Cards;
        
        [Header("Inventory Optional")]
        public List<Button> CardsInventory;

        [Header("Inventory Setup")]
        [SerializeField] private RectTransform inventoryPanel;

        public RectTransform InventoryField;
        public RectTransform UsedCardTempPanel;
        public Button InventoryButton;
        [SerializeField] private RectTransform usedCardField;

        [HideInInspector] 
        public Button UsedCardTemp;

        [HideInInspector]
        public bool IsBuildMode;
        
        private bool toggle;

        private bool isInventoryFull;

        public static InventoryManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            UsedCardTempPanel.gameObject.SetActive(false);
            inventoryPanel.gameObject.SetActive(false);
            IsBuildMode = false;
            isInventoryFull = false;
            RandomScrollManager.Instance.AddCard += AddCardToInventory;
        }

        private void AddCardToInventory()
        {
            var count = CardsInventory.Count(item => item != null);
            if (count >= CardsInventory.Count)
            {
                UiManager.Instance.SetTextNotification("Card Inventory is full");
                UiManager.Instance.GetNotification();
                return;
            }

            for (var i = 0; i < CardsInventory.Count; i++)
            {
                if (CardsInventory[i] == null)
                {
                    CardsInventory[i] = Instantiate(RandomScrollManager.Instance.CardTemp,InventoryField);
                    TagCardId(CardsInventory[i]);
                    UiManager.Instance.OnCloseRandomButtonClicked();
                }
                else
                {
                    continue;
                } 
                return;
            }
        }

        private void TagCardId(Button button)
        {
            if (RandomScrollManager.Instance.CardTemp.CompareTag("Card_F81"))
            {
                AddCard(HeroName.F81_Hermes,Cards[0],button);
            }
            else if (RandomScrollManager.Instance.CardTemp.CompareTag("Card_F13"))
            {
                AddCard(HeroName.F13_Colorado,Cards[1],button);
            }
            else if (RandomScrollManager.Instance.CardTemp.CompareTag("Card_W75"))
            {
                AddCard(HeroName.W75_Valiant,Cards[2],button);
            }
            else if (RandomScrollManager.Instance.CardTemp.CompareTag("Card_W410"))
            {
                AddCard(HeroName.W410_Odin,Cards[3],button);
            }
            else if (RandomScrollManager.Instance.CardTemp.CompareTag("Card_TEP58"))
            {
                AddCard(HeroName.TEP58_Interpid,Cards[4],button);
            }
            else if (RandomScrollManager.Instance.CardTemp.CompareTag("Card_P32"))
            {
                AddCard(HeroName.P32_Enterprise,Cards[5],button);
            }
            else if (RandomScrollManager.Instance.CardTemp.CompareTag("Card_Z1"))
            {
                AddCard(HeroName.Z1_Hornet,Cards[6],button);
            }
            else if (RandomScrollManager.Instance.CardTemp.CompareTag("Card_Z23"))
            {
                AddCard(HeroName.Z23_Glorious,Cards[7],button);
            }
            else if (RandomScrollManager.Instance.CardTemp.CompareTag("Card_Z25"))
            {
                AddCard(HeroName.Z25_Terror,Cards[8],button);
            }
            else if (RandomScrollManager.Instance.CardTemp.CompareTag("Card_Z46"))
            {
                AddCard(HeroName.Z46_Genesis,Cards[9],button);
            }
            else if (RandomScrollManager.Instance.CardTemp.CompareTag("Card_U73"))
            {
                AddCard(HeroName.U73_Aurora,Cards[10],button);
            }
            else if (RandomScrollManager.Instance.CardTemp.CompareTag("Card_U556"))
            {
                AddCard(HeroName.U556_Radford,Cards[11],button);
            }
            else if (RandomScrollManager.Instance.CardTemp.CompareTag("Card_U110"))
            {
                AddCard(HeroName.U110_Sendai,Cards[12],button);
            }
            else if (RandomScrollManager.Instance.CardTemp.CompareTag("Card_U101"))
            {
                AddCard(HeroName.U101_Jupiter,Cards[13],button);
            }
            else if (RandomScrollManager.Instance.CardTemp.CompareTag("Card_I168"))
            {
                AddCard(HeroName.I168_Memphis,Cards[14],button);
            }
            else if (RandomScrollManager.Instance.CardTemp.CompareTag("Card_I13"))
            {
                AddCard(HeroName.I13_Centaur,Cards[15],button);
            }
            else if (RandomScrollManager.Instance.CardTemp.CompareTag("Card_I58"))
            {
                AddCard(HeroName.I58_Essex,Cards[16],button);
            }
            else if (RandomScrollManager.Instance.CardTemp.CompareTag("Card_I26"))
            {
                AddCard(HeroName.I26_Albacore,Cards[17],button);
            }
        }

        private void AddCard(HeroName cardName,CardBluePrint card,Button button)
        {
            if (isInventoryFull)
            {
                return;
            }
            if (card.CardName == cardName)
            {
                card.Card = button;
                card.Card.onClick.AddListener(delegate { GetCard(cardName,card,button); });
                card.CardAmount++;
                card.CardToEvo.Add(button);
            }
        }
        
        private void GetCard(HeroName cardName,CardBluePrint card,Button button)
        {
            if (card.CardName == cardName)
            {
                PickHero(cardName);

                if (BuildManager.Instance.IsHeroBuilt)
                {
                    UiManager.Instance.SetTextNotification("This hero has been build");
                    UiManager.Instance.GetNotification();
                    return; //tell Player herobuilt
                }

                if (card.IsHeroCard)
                {
                    if (Area.CurrentHeroAmount >= BuildManager.Instance.HeroLimit)
                    {
                        UiManager.Instance.SetTextNotification("Can't build \nHero anymore");
                        UiManager.Instance.GetNotification();
                        return; //tell Player Herofull
                    }
                }

                if (!card.IsHeroCard)
                {
                    if (Area.CurrentSubHeroAmount >= BuildManager.Instance.SubHeroLimit)
                    {
                        UiManager.Instance.SetTextNotification("Can't build \nSub-Hero anymore");
                        UiManager.Instance.GetNotification();
                        return; //tell Player SubHerofull
                    }
                }

                IsBuildMode = true;
                AnimationManager.Instance.MapUp();
                UiManager.Instance.blockUI.gameObject.SetActive(true);

                //To Hide other card when card has selected
                // var cardTemps = CardsInventory.FindAll(x => x.CompareTag($"Card_{cardName}"));
                // foreach (var cardTemp in cardTemps) 
                // {
                //     cardTemp.interactable = false;
                // }
                

                
                UsedCardTempPanel.gameObject.SetActive(true);



                UsedCardTemp = Instantiate(button,usedCardField);
                UsedCardTemp.onClick.RemoveAllListeners();
                UsedCardTemp.onClick.AddListener(delegate { CancelCardUsed(cardName,button,card); });

                card.CardAmount--;
                Destroy(button.gameObject);
                
                ShowInventoryUI();

                
            }
        }
        
        private void CancelCardUsed(HeroName cardName, Button button, CardBluePrint card)
        {
            UsedCardTempPanel.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
            UsedCardTemp.onClick.RemoveAllListeners();

            UsedCardTemp.onClick.AddListener(delegate { GetCard(cardName,card,button); });
            ShowInventoryUI();
            
            RandomScrollManager.Instance.CardTemp = UsedCardTemp;
            AddCardToInventory();
            
            AnimationManager.Instance.MapDown();
            IsBuildMode = false;
            
            BuildManager.Instance.heroToBuild = HeroTeam.Find(x=>x.HeroName == HeroName.Null);
            UiManager.Instance.blockUI.gameObject.SetActive(!true);
            Destroy(UsedCardTemp.gameObject);
            UsedCardTemp = null;
        }

        private void GetHero(HeroName name)
        {
            //find hero in list
            var hero = HeroTeam.Find(x => x.HeroName == name);
            BuildManager.Instance.SetHero(hero);
        }
        
        private void PickHero(HeroName heroName)
        {
            switch (heroName)
            {
                case HeroName.F81_Hermes:
                    GetHero(HeroName.F81_Hermes);
                    break;
                case HeroName.F13_Colorado:
                    GetHero(HeroName.F13_Colorado);
                    break;
                case HeroName.W75_Valiant:
                    GetHero(HeroName.W75_Valiant);
                    break;
                case HeroName.W410_Odin:
                    GetHero(HeroName.W410_Odin);
                    break;
                case HeroName.TEP58_Interpid:
                    GetHero(HeroName.TEP58_Interpid);
                    break;
                case HeroName.P32_Enterprise:
                    GetHero(HeroName.P32_Enterprise);
                    break;
                case HeroName.Z1_Hornet:
                    GetHero(HeroName.Z1_Hornet);
                    break;
                case HeroName.Z23_Glorious:
                    GetHero(HeroName.Z23_Glorious);
                    break;
                case HeroName.Z25_Terror:
                    GetHero(HeroName.Z25_Terror);
                    break;
                case HeroName.Z46_Genesis:
                    GetHero(HeroName.Z46_Genesis);
                    break;
                case HeroName.U73_Aurora:
                    GetHero(HeroName.U73_Aurora);
                    break;
                case HeroName.U556_Radford:
                    GetHero(HeroName.U556_Radford);
                    break;
                case HeroName.U110_Sendai:
                    GetHero(HeroName.U110_Sendai);
                    break;
                case HeroName.U101_Jupiter:
                    GetHero(HeroName.U101_Jupiter);
                    break;
                case HeroName.I168_Memphis:
                    GetHero(HeroName.I168_Memphis);
                    break;
                case HeroName.I13_Centaur:
                    GetHero(HeroName.I13_Centaur);
                    break;
                case HeroName.I58_Essex:
                    GetHero(HeroName.I58_Essex);
                    break;
                case HeroName.I26_Albacore:
                    GetHero(HeroName.I26_Albacore);
                    break;
            }
        }

        public void ShowInventoryUI()
        {
            UiManager.Instance.UpdateCardInventory();
            toggle = !toggle;
            if (toggle)
            {
                RandomScrollManager.Instance.HideRandomScollButton.Invoke();
                inventoryPanel.gameObject.SetActive(true);
                RandomScrollManager.Instance.LongClickCheck = false;
                BuildManager.Instance.DeselectHero();
            }
            else
            {
                RandomScrollManager.Instance.ShowRandomScollButton.Invoke();
                inventoryPanel.gameObject.SetActive(!true);

                if (UsedCardTempPanel.gameObject.activeInHierarchy)
                {
                    UiManager.Instance.RandomScrollButtonArea.gameObject.SetActive(false);
                    InventoryButton.gameObject.SetActive(false);
                    RandomScrollManager.Instance.LongClickCheck = true;
                }
                else
                {
                    UiManager.Instance.RandomScrollButtonArea.gameObject.SetActive(true);
                    InventoryButton.gameObject.SetActive(true);
                    RandomScrollManager.Instance.LongClickCheck = false;
                }

            }
        }
    }
}

