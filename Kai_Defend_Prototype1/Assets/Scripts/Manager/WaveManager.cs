using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Manager
{
    public class WaveManager : MonoBehaviour
    {
        public static WaveManager Instance { get; private set; }

        [SerializeField] private float timePerWaves;
        [SerializeField] private float timeCooldownWaves;
        [SerializeField] private float timer;

        private float timerTemp;
        
        public float CheatHpTemp;
        
        public float Countdown
        {
            get => timer;
            private set => timer = value;
        }
        
        //TODO: Implement : public event Action GameStarted;
        public event Action OnNextWave;

        private bool preparePhase;

        public double WaveIndex;

        public float TimerGameover;
        
        [HideInInspector]
        public int RandomEnemy;
        
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
            GameManager.Instance.GameOver += GetWaveAndTimeGameOver;
            TimerGameover = 0;
            RandomEnemy = UnityEngine.Random.Range(1, 4);
            GameManager.Instance.UpdateEnemyInfo();
            WaveIndex = 1;
            preparePhase = true;
            timerTemp = timer;
        }

        private void Update()
        { 
            TimerGameover += Time.deltaTime;
           WaveManage();
           WorldUIManager.Instance.WaveTimerUI.fillAmount = Countdown / timerTemp;
        }

        private void GetWaveAndTimeGameOver()
        {
            UiManager.Instance.SetWaveSurvived($"{WaveIndex - 1}");
            UiManager.Instance.SetTimeSurvived($"{TimerGameover:F2}");
        }

        private void WaveManage()
        {
            if (Countdown <= 0f && preparePhase)
            {
                if (WaveIndex % 10 == 0)
                {
                    timeCooldownWaves -= 10;
                }
                
                switch (RandomEnemy)
                {
                    case 1 :
                        GameManager.Instance.Start_Colubted_Wave();
                        break;
                    case 2 :
                        GameManager.Instance.Start_Embio_Wave();
                        break;
                    case 3 :
                        GameManager.Instance.Start_Eclipseside_Wave();
                        break;
                    case 4 :
                        GameManager.Instance.Start_CrossDive_Wave();
                        break;
                    case 5 :
                        GameManager.Instance.Start_Shiro_Wave();
                        break;
                    case 6 :
                        GameManager.Instance.Start_Kana_Wave();
                        break;
                    case 7 :
                        GameManager.Instance.Start_Chest_Wave();
                        break;
                }
                WorldUIManager.Instance.WaveTextHolder.text = "Battle Phase";
                Countdown = timePerWaves;
                timerTemp = Countdown;
                preparePhase = false;
                WaveIndex++; //dont forget to set wave = 1 when restart
            }
            else if (Countdown <= 0f && preparePhase == false)
            {
                if (GameManager.Instance.ChestTemp != null)
                {
                    MoneyManager.Instance.AddMoney((int)CheatHpTemp * 2);
                    CheatHpTemp = 0;

                    Destroy(GameManager.Instance.ChestTemp.gameObject);
                    GameManager.Instance.ChestTemp = null;
                    //TODO : Implement Enemy Chest
                }

                if (WaveIndex % 10 == 0)
                {
                    timeCooldownWaves += 10;
                }

                if (WaveIndex % 5 == 0 )
                {
                    RandomEnemy = 7;
                }
                else
                {
                    RandomEnemy = UnityEngine.Random.Range(1, 7);
                }
                UiManager.Instance.SetWaveCount($"{WaveIndex}");
                
                WorldUIManager.Instance.WaveTextHolder.text = "Prepare Phase";
                OnNextWave?.Invoke();
                GameManager.Instance.UpdateEnemyInfo();

                GameManager.Instance.End_Wave();//instead of the End All wave 
                Countdown = timeCooldownWaves;
                timerTemp = Countdown;
                preparePhase = true;
            }

            Countdown -= Time.deltaTime;

            Countdown = Mathf.Clamp(Countdown, 0f, Mathf.Infinity);

            //TODO:Random Wave
        }
    }
}

