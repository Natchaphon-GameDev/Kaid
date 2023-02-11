using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class MoneyManager : MonoBehaviour
    {
        public static MoneyManager Instance { get; private set; }
        
        public int Money { get; set; }
        [SerializeField] private int startMoney;
        public int MaxMoney;

        public void AddMoney(int amount)
        {
            UiManager.Instance.SetTextNotification($"You Receive Money\n{amount} $");
            UiManager.Instance.GetNotification();
            if ((Money + amount) >= MaxMoney)
            {
                UiManager.Instance.SetTextNotification("Your Wallet is Full");
                UiManager.Instance.GetNotification();
                Money = MaxMoney;
                return;
            }
            Money += amount;
        }
        
        public void DeleteMoney(int amount)
        {
            Money -= amount;
        }
        
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
            Money = startMoney;
        }
    }
}

