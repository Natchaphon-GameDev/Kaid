using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        public event Action GameOver;
        
        public int EnemyCount { get; set; }

        private bool IsMoreThanEnemyLimit;

        public int GetCountdownOverTime => (int)countdownGameOver;

        private float resetTime;

        [Header("Set Enemy Limit")]
        [SerializeField] private float countdownGameOver;
        public float EnemyLimit = 32;


        [Header("Set Enemy Upgrade In 5 Wave")] 
        [SerializeField] private int upgradeEnemyIn5Wave = 5;
        
        [Header("Set EvoHero Status")] 
        public float UpgradeAttackDamagePreEvo = 50;
        public float UpgradeAttackRangePreEvo = 1;
        
        [Header("Colubted")]
        [SerializeField] private string colubted_Name;
        [SerializeField] private float colubted_MoveSpeed;
        [SerializeField] private float colubted_Health;
        [SerializeField] private int colubted_GoldDrop;
        [SerializeField] private Element colubted_EnemyElement;
        [SerializeField] private float colubted_SpawnTime;
        [SerializeField] private Colubted colubtedPrefab;
        
        [Header("Embio")]
        [SerializeField] private string embio_Name;
        [SerializeField] private float embio_MoveSpeed;
        [SerializeField] private float embio_Health;
        [SerializeField] private int embio_GoldDrop;
        [SerializeField] private Element embio_EnemyElement;
        [SerializeField] private float embio_SpawnTime;
        [SerializeField] private Embio embioPrefab;
        
        [Header("Eclipseside")]
        [SerializeField] private string eclipseside_Name;
        [SerializeField] private float eclipseside_MoveSpeed;
        [SerializeField] private float eclipseside_Health;
        [SerializeField] private int eclipseside_GoldDrop;
        [SerializeField] private Element eclipseside_EnemyElement;
        [SerializeField] private float eclipseside_SpawnTime;
        [SerializeField] private Eclipseside eclipsesidePrefab;
        
        [Header("CrossDive")]
        [SerializeField] private string crossDive_Name;
        [SerializeField] private float crossDive_MoveSpeed;
        [SerializeField] private float crossDive_Health;
        [SerializeField] private int crossDive_GoldDrop;
        [SerializeField] private Element crossDive_EnemyElement;
        [SerializeField] private float crossDive_SpawnTime;
        [SerializeField] private CrossDive crossDivePrefab;
        
        [Header("Shiro")]
        [SerializeField] private string shiro_Name;
        [SerializeField] private float shiro_MoveSpeed;
        [SerializeField] private float shiro_Health;
        [SerializeField] private int shiro_GoldDrop;
        [SerializeField] private Element shiro_EnemyElement;
        [SerializeField] private float shiro_SpawnTime;
        [SerializeField] private Shiro shiroPrefab;
        
        [Header("Kana")]
        [SerializeField] private string kana_Name;
        [SerializeField] private float kana_MoveSpeed;
        [SerializeField] private float kana_Health;
        [SerializeField] private int kana_GoldDrop;
        [SerializeField] private Element kana_EnemyElement;
        [SerializeField] private float kana_SpawnTime;
        [SerializeField] private Kana kanaPrefab;
        
        [Header("Chest")]
        [SerializeField] private string chest_Name;
        [SerializeField] private float chest_MoveSpeed;
        [SerializeField] private float chest_Health;
        [SerializeField] private int chest_GoldDrop;
        [SerializeField] private Element chest_EnemyElement;
        [SerializeField] private float chest_SpawnTime;
        [SerializeField] private Chest chestPrefab;

        public Chest ChestTemp;
        
        [Header("Upgrade Enemy Per Wave")]
        [SerializeField] private float enemyUpgrade_HP;
        [SerializeField] private int enemyUpgrade_Gold;
        [SerializeField] private float enemyUpgrade_Speed;

        

        // public event Action EnemyA_Spawned;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            IsMoreThanEnemyLimit = false;
            EnemyCount = 0;
            resetTime = countdownGameOver;
        }

        private void Start()
        {
            WaveManager.Instance.OnNextWave += UpgradeEnemy;
            // AdsManager.Instance.OnAdsReward += OnAdsReward;
        }

        public void OnAdsReward()
        {
            countdownGameOver = resetTime;
        }

        private void Update()
        {
            EnemyMoreThanLimit();
            GameEnded();
        }

        private void UpgradeEnemy() //TODO : Refactor this code
        {
            if (WaveManager.Instance.WaveIndex % 5 == 0)
            {
                enemyUpgrade_HP += upgradeEnemyIn5Wave;
                enemyUpgrade_Gold += upgradeEnemyIn5Wave;
            }
            else
            {
                //HP
                colubted_Health += enemyUpgrade_HP;
                embio_Health += enemyUpgrade_HP;
                eclipseside_Health += enemyUpgrade_HP;
                crossDive_Health += enemyUpgrade_HP;
                shiro_Health += enemyUpgrade_HP;
                kana_Health += enemyUpgrade_HP;
                
                //Gold
                colubted_GoldDrop += enemyUpgrade_Gold;
                embio_GoldDrop += enemyUpgrade_Gold;
                eclipseside_GoldDrop += enemyUpgrade_Gold;
                crossDive_GoldDrop += enemyUpgrade_Gold;
                shiro_GoldDrop += enemyUpgrade_Gold;
                kana_GoldDrop += enemyUpgrade_Gold;
            }

            if (WaveManager.Instance.WaveIndex % 10 == 0)
            {
                colubted_MoveSpeed += enemyUpgrade_Speed;
                embio_MoveSpeed += enemyUpgrade_Speed;
                eclipseside_MoveSpeed += enemyUpgrade_Speed;
                crossDive_MoveSpeed += enemyUpgrade_Speed;
                shiro_MoveSpeed += enemyUpgrade_Speed;
                kana_MoveSpeed += enemyUpgrade_Speed;

                //Debug Revents Enemy from going faster than bullet. 
                foreach (var hero in InventoryManager.Instance.HeroTeam)
                {
                    if (hero.AttackType == AttackType.Laser)
                    {
                        continue;
                    }
                    hero.BulletSpeed += enemyUpgrade_Speed;
                }
            }
        }

        private static void GetEnemyPicture(int index)
        {
            var picTemp = WorldUIManager.Instance.EnemyPictures[index];
            foreach (var picture in WorldUIManager.Instance.EnemyPictures)
            {
                if (picture == picTemp)
                {
                    picture.gameObject.SetActive(true);
                    continue;
                }
                picture.gameObject.SetActive(!true);
            }
        }
        
        public void UpdateEnemyInfo()
        {
            var ui = WorldUIManager.Instance;
            
            switch (WaveManager.Instance.RandomEnemy)
            {
                case 1:
                    GetEnemyPicture(0);
                    ui.UpdateEnemyInfoText($"Name : {colubted_Name} \n\n" +
                                           $"HP : {colubted_Health} \n" +
                                           $"Element : {colubted_EnemyElement.ToString()} \n" +
                                           $"Speed : {colubted_MoveSpeed} \n" +
                                           $"Gold Drop : {colubted_GoldDrop}");
                    break;
                case 2:
                    GetEnemyPicture(1);
                    ui.UpdateEnemyInfoText($"Name : {embio_Name} \n\n" +
                                           $"HP : {embio_Health} \n" +
                                           $"Element : {embio_EnemyElement.ToString()} \n" +
                                           $"Speed : {embio_MoveSpeed} \n" +
                                           $"Gold Drop : {embio_GoldDrop}");
                    break;
                case 3:
                    GetEnemyPicture(2);
                    ui.UpdateEnemyInfoText($"Name : {eclipseside_Name} \n\n" +
                                           $"HP : {eclipseside_Health} \n" +
                                           $"Element : {eclipseside_EnemyElement.ToString()} \n" +
                                           $"Speed : {eclipseside_MoveSpeed} \n" +
                                           $"Gold Drop : {eclipseside_GoldDrop}");
                    break;
                case 4:
                    GetEnemyPicture(3);
                    ui.UpdateEnemyInfoText($"Name : {crossDive_Name} \n\n" +
                                           $"HP : {crossDive_Health} \n" +
                                           $"Element : {crossDive_EnemyElement.ToString()} \n" +
                                           $"Speed : {crossDive_MoveSpeed} \n" +
                                           $"Gold Drop : {crossDive_GoldDrop}");
                    break;
                case 5:
                    GetEnemyPicture(4);
                    ui.UpdateEnemyInfoText($"Name : {shiro_Name} \n\n" +
                                           $"HP : {shiro_Health} \n" +
                                           $"Element : {shiro_EnemyElement.ToString()} \n" +
                                           $"Speed : {shiro_MoveSpeed} \n" +
                                           $"Gold Drop : {shiro_GoldDrop}");
                    break;
                case 6:
                    GetEnemyPicture(5);
                    ui.UpdateEnemyInfoText($"Name : {kana_Name} \n\n" +
                                           $"HP : {kana_Health} \n" +
                                           $"Element : {kana_EnemyElement.ToString()} \n" +
                                           $"Speed : {kana_MoveSpeed} \n" +
                                           $"Gold Drop : {kana_GoldDrop}");
                    break;
                case 7:
                    GetEnemyPicture(6);
                    ui.UpdateEnemyInfoText($"Name : {chest_Name} \n\n" +
                                           $"HP : {chest_Health} \n" +
                                           $"Element : {chest_EnemyElement.ToString()} \n" +
                                           $"Speed : {chest_MoveSpeed} \n" +
                                           $"Gold Drop : {chest_GoldDrop}");
                    break;
            }
        }
        
        private static void GetHeroPicture(int index)
        {
            var picTemp = WorldUIManager.Instance.HeroPictures[index];
            foreach (var picture in WorldUIManager.Instance.HeroPictures)
            {
                if (picture == picTemp)
                {
                    picture.gameObject.SetActive(true);
                    continue;
                }
                picture.gameObject.SetActive(!true);
            }
        }

        public void UpdateHeroInfo()
        {
            var ui = WorldUIManager.Instance;
            var hero = BuildManager.Instance.heroToBuild;
            var getIndex = InventoryManager.Instance.HeroTeam.IndexOf(hero);

            switch (hero.AttackType)
            {
                case AttackType.MachineGun:
                    GetHeroPicture(getIndex);
                    ui.UpdateHeroInfoText($"{hero.HeroName.ToString()} \n\n" +
                                          $"[ {hero.AttackType.ToString()} ] \n" +
                                          $"Element : {hero.ElementType.ToString()} \n" +
                                          $"Atk Dmg : {Mathf.Floor(hero.AttackDamage)} \n" +
                                          $"Atk Spd : {hero.AttackSpeed:F1} \n" +
                                          $"Upgrade : {hero.UpgradeCost} $");
                    break;
                case AttackType.Missile:
                    GetHeroPicture(getIndex);
                    ui.UpdateHeroInfoText($"{hero.HeroName.ToString()} \n\n" +
                                          $"[ {hero.AttackType.ToString()} ] \n" +
                                          $"Element : {hero.ElementType.ToString()} \n" +
                                          $"Atk Dmg : {Mathf.Floor(hero.AttackDamage)} \n" +
                                          $"Atk Spd : {hero.AttackSpeed:F1} \n" +
                                          $"Aoe Range : {Mathf.Floor(hero.AoeRange)} \n" +
                                          $"Upgrade : {hero.UpgradeCost} $");
                    break;
                case AttackType.Laser:
                    GetHeroPicture(getIndex);
                    ui.UpdateHeroInfoText($"{hero.HeroName.ToString()} \n\n" +
                                          $"[ {hero.AttackType.ToString()} ] \n" +
                                          $"Element : {hero.ElementType.ToString()} \n" +
                                          $"Atk Dmg : {Mathf.Floor(hero.DamageOverTime)} \n" +
                                          $"Slow: {Mathf.Floor(hero.SlowPercent * 100)}% \n" +
                                          $"Upgrade : {hero.UpgradeCost} $");
                    break;
            }
        }
        
        private void Spawn_Colubted()
        {
            var spawnEnemy = Instantiate(colubtedPrefab);
            spawnEnemy.Init(colubted_MoveSpeed,colubted_Health,colubted_GoldDrop,colubted_EnemyElement);

            EnemyCount++;
        }
        
        private void Spawn_Embio()
        {
            var spawnEnemy = Instantiate(embioPrefab);
            spawnEnemy.Init(embio_MoveSpeed,embio_Health,embio_GoldDrop,embio_EnemyElement);

            EnemyCount++;
        }
        
        private void Spawn_Eclipseside()
        {
            var spawnEnemy = Instantiate(eclipsesidePrefab);
            spawnEnemy.Init(eclipseside_MoveSpeed,eclipseside_Health,eclipseside_GoldDrop,eclipseside_EnemyElement);

            EnemyCount++;
        }
        
        private void Spawn_CrossDive()
        {
            var spawnEnemy = Instantiate(crossDivePrefab);
            spawnEnemy.Init(crossDive_MoveSpeed,crossDive_Health,crossDive_GoldDrop,crossDive_EnemyElement);

            EnemyCount++;
        }
        
        private void Spawn_Shiro()
        {
            var spawnEnemy = Instantiate(shiroPrefab);
            spawnEnemy.Init(shiro_MoveSpeed, shiro_Health, shiro_GoldDrop, shiro_EnemyElement);
            
            EnemyCount++;
        }
        
        private void Spawn_Kana()
        {
            var spawnEnemy = Instantiate(kanaPrefab);
            spawnEnemy.Init(kana_MoveSpeed, kana_Health, kana_GoldDrop, kana_EnemyElement);
            
            EnemyCount++;
        }


        private void Spawn_Chest()
        {
            ChestTemp = Instantiate(chestPrefab);
            ChestTemp.Init(chest_MoveSpeed,chest_Health,chest_GoldDrop,chest_EnemyElement);
        
            EnemyCount++;
        }

        private void EnemyMoreThanLimit()
        {
            if (EnemyCount >= EnemyLimit)
            {
                IsMoreThanEnemyLimit = true;
                countdownGameOver -= Time.deltaTime;
                UiManager.Instance.GetCountDownGameOver();
            }
            
            if (EnemyCount < EnemyLimit)
            {
                IsMoreThanEnemyLimit = false;
                countdownGameOver = resetTime;
                UiManager.Instance.HideTimerNotification();
            }
        }

        private void GameEnded()
        {
            if (countdownGameOver <= 0f && IsMoreThanEnemyLimit)
            {
                GameOver.Invoke();
                // FirebaseManager.Instance.UpdateToDB();
            }
        }

        public void Start_Colubted_Wave()
        {
            InvokeRepeating("Spawn_Colubted", WaveManager.Instance.Countdown, colubted_SpawnTime);
        }
        
        public void End_Wave()
        {
            CancelInvoke("Spawn_Colubted");
            CancelInvoke("Spawn_Embio");
            CancelInvoke("Spawn_Eclipseside");
            CancelInvoke("Spawn_CrossDive");
            CancelInvoke("Spawn_Shiro");
            CancelInvoke("Spawn_Kana");
            CancelInvoke("Spawn_Chest");
        }
        
        public void Start_Embio_Wave()
        {
            InvokeRepeating("Spawn_Embio", WaveManager.Instance.Countdown, embio_SpawnTime);
        }
        
        public void Start_Eclipseside_Wave()
        {
            InvokeRepeating("Spawn_Eclipseside", WaveManager.Instance.Countdown, eclipseside_SpawnTime);
        }
        
        public void Start_CrossDive_Wave()
        {
            InvokeRepeating("Spawn_CrossDive", WaveManager.Instance.Countdown, crossDive_SpawnTime);
        }
        
        public void Start_Shiro_Wave()
        {
            InvokeRepeating("Spawn_Shiro", WaveManager.Instance.Countdown, shiro_SpawnTime);
        }
        
        public void Start_Kana_Wave()
        {
            InvokeRepeating("Spawn_Kana", WaveManager.Instance.Countdown, kana_SpawnTime);
        }
        
        public void Start_Chest_Wave()
        {
            InvokeRepeating("Spawn_Chest", WaveManager.Instance.Countdown, chest_SpawnTime);
        }

        public void GotoMainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
    
    

}
