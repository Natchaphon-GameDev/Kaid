using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Manager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Manager
{
    public class RandomScrollManager : MonoBehaviour
{
    public static RandomScrollManager Instance { get; private set; }
    
    [SerializeField] private List<Button> heroPrefabs;
    [SerializeField] private List<Button> subHeroPrefabs;
    [Header("Setting")]
    [SerializeField] private float heroRandomRate = 0.8f;
    [SerializeField] private float subHeroRandomRate = 0.2f;
    [SerializeField] private float heroRandomPlusRate = 0.6f;
    [SerializeField] private float subHeroRandomPlusRate = 0.4f;
    [SerializeField] private RectTransform randomField;
    [SerializeField] private RectTransform randomArea;
    [SerializeField] private int maxCard = 3;
    public int RandomScrollCost = 100;
    public int RandomScrollPlusCost = 300;

    public bool NotEnoughMoneyScore => MoneyManager.Instance.Money < RandomScrollCost;
    public bool NotEnoughMoneyScorePlus => MoneyManager.Instance.Money < RandomScrollPlusCost;
    
    
    [SerializeField] private List<Button> cards;
    
    public UnityEvent HideRandomScollButton;
    public UnityEvent ShowRandomScollButton;

    public event Action OnFirstRandomCard;

    private bool isPickedFirstCard;

    [HideInInspector]
    public bool LongClickCheck;

    [HideInInspector]
    public Button CardTemp;

    public event Action AddCard;

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

    private void Start()
    {
        randomArea.gameObject.SetActive(true);
        TimeManager.Instance.DisableControler = true;
        RandomCardOnStart();
    }

    private void RandomCardOnStart()
    {
        LongClickCheck = true;
        for (var i = 0; i < maxCard; i++)
        {
            cards.Add(Instantiate(heroPrefabs[Random.Range(0, heroPrefabs.Count)],randomField));
        }
        
        cards[0].onClick.AddListener(AddCard1);
        cards[1].onClick.AddListener(AddCard2);
        cards[2].onClick.AddListener(AddCard3);
        isPickedFirstCard = true;
    }

    private void ClearCard()
    {
        UiManager.Instance.UpdateCardInventory();

        foreach (var card in cards)
        {
            Destroy(card.gameObject);
        }
        cards.Clear();
    }

    private void RandomCard()
    {
        LongClickCheck = true;
        var cardCount = 0;
        if (!UiManager.Instance.IsRandomPlus)
        {
            if (NotEnoughMoneyScore)
            {
                return;
            }
            HideRandomScollButton.Invoke();
            MoneyManager.Instance.DeleteMoney(RandomScrollCost);
            while(cardCount < maxCard)
            {
                if (Random.value > subHeroRandomRate) 
                {
                    cards.Add(Instantiate(subHeroPrefabs[Random.Range(0, subHeroPrefabs.Count)],randomField));
                    cardCount++;
                }
                else if(Random.value > heroRandomRate) 
                {
                    cards.Add(Instantiate(heroPrefabs[Random.Range(0, heroPrefabs.Count)],randomField));
                    cardCount++;
                }
            }
        }
        else
        {
            if (NotEnoughMoneyScorePlus)
            {
                return;
            }
            HideRandomScollButton.Invoke();
            MoneyManager.Instance.DeleteMoney(RandomScrollPlusCost);
            while(cardCount < maxCard)
            {
                if (Random.value > subHeroRandomPlusRate) 
                {
                    cards.Add(Instantiate(subHeroPrefabs[Random.Range(0, subHeroPrefabs.Count)],randomField));
                    cardCount++;
                }
                else if(Random.value > heroRandomPlusRate) 
                {
                    cards.Add(Instantiate(heroPrefabs[Random.Range(0, heroPrefabs.Count)],randomField));
                    cardCount++;
                }
            }
        }
        
        cards[0].onClick.AddListener(AddCard1);
        cards[1].onClick.AddListener(AddCard2);
        cards[2].onClick.AddListener(AddCard3);
    }

    public void ShowCard()
    {
        RandomCard();
        randomArea.gameObject.SetActive(true);
    }

    public void HideCard()
    {
        if (isPickedFirstCard)
        {
            TimeManager.Instance.DisableControler = false;
            OnFirstRandomCard.Invoke();
        }
        isPickedFirstCard = false;
        
        LongClickCheck = false;

        ClearCard();
        randomArea.gameObject.SetActive(!true);
    }

    public void ReRoll()
    {
        if (UiManager.Instance.IsRandomPlus)
        {
            if (NotEnoughMoneyScorePlus)
            {
                UiManager.Instance.NotEnoughMoneyUI();
                return;
            }
        }
        else
        {
            if (NotEnoughMoneyScore)
            {
                UiManager.Instance.NotEnoughMoneyUI();
                return;
            }
        }
        ClearCard();
        RandomCard();
    }

    private void AddCard1()
    {
        CardTemp = cards[0];
        AddCard.Invoke();
    }
    private void AddCard2()
    {
        CardTemp = cards[1];
        AddCard.Invoke();
    } 
    private void AddCard3()
    {
        CardTemp = cards[2];
        AddCard.Invoke();
    }
}
}
