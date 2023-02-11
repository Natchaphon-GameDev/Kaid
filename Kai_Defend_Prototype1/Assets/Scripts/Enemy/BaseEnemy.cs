using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using Map;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public class BaseEnemy : MonoBehaviour
    {
        private float enemySpeedTemp;
        public float EnemySpeed { get; private set; }
        public float EnemyHp { get; private set; }
        public int GoldDrop { get; private set; }
        public Element EnemyElement { get; private set; }

        [SerializeField] protected Image healthBar;
        [SerializeField] protected GameObject popUpDamageText;
        [SerializeField] protected Canvas rotateCanvas;

        private float calDamage;
        
        private Area areaTarget;

        private float healthTranform;

        private bool isAlive;

        private Transform target;
        private int wayPointIndex;
        
        private void Start()
        {
            transform.LookAt(EnemyWayPoint.Instance.WayPoints[1].gameObject.transform);
            enemySpeedTemp = EnemySpeed;
            healthTranform = EnemyHp;
            target = EnemyWayPoint.Instance.WayPoints[0];
            isAlive = true;
        }

        protected void TakeDamage(float amount)
        {
            healthTranform -= amount;
            healthBar.fillAmount = healthTranform / EnemyHp;
            
            //Pop up damage 
            if (PopUpDamage.CheckLaserType)
            {
                calDamage += amount;
                StartCoroutine(DelayLaserPopUp());
            }
            else
            {
                var indicator = Instantiate(popUpDamageText, transform.position, Quaternion.identity).GetComponent<PopUpDamage>();
                indicator.SetDamageText(amount);
            }
            
            UiManager.Instance.DamageDoneGameOver += amount;

            if (healthTranform <= 0 && isAlive)
            {
                EnemyDeath();
            }
        }

        private IEnumerator DelayLaserPopUp()
        {
            yield return new WaitForSeconds(0.3f);
            var indicator = Instantiate(popUpDamageText, transform.position, Quaternion.identity).GetComponent<PopUpDamage>();
            indicator.SetDamageText(calDamage);
            calDamage = 0;
            StopAllCoroutines();
        }

        public void Slow(float percent)
        {
            EnemySpeed = enemySpeedTemp * (1f - percent);
        }

        private void EnemyDeath()
        {
            if (MoneyManager.Instance.Money >= MoneyManager.Instance.MaxMoney)
            {
                UiManager.Instance.SetTextNotification("Your Wallet is Full");
                UiManager.Instance.GetNotification();
                //set money = max
                MoneyManager.Instance.Money = MoneyManager.Instance.MaxMoney;
            }
            else
            {
                MoneyManager.Instance.Money += GoldDrop;
            }
            isAlive = false;
            GameManager.Instance.EnemyCount--;
            Destroy(gameObject);
        }
        protected void Move()
        {
            var direction = target.position - transform.position;
            transform.Translate(direction.normalized * EnemySpeed * Time.deltaTime, Space.World);
        
            if (Vector3.Distance(transform.position, target.position) <= 0.1f)
            {
                GoToNextWayPoint();
            }

            //Set Speed to default after laser shoot
            EnemySpeed = enemySpeedTemp;
        }
        
        private void GoToNextWayPoint()
        {
            if (wayPointIndex >= EnemyWayPoint.Instance.WayPoints.Length - 1)
            {
                wayPointIndex = 1;
            }
            wayPointIndex++;
            transform.LookAt(EnemyWayPoint.Instance.WayPoints[wayPointIndex].gameObject.transform);
            target = EnemyWayPoint.Instance.WayPoints[wayPointIndex];
        }


        protected void Init(float enemySpeed, float enemyHp, int goldDrop, Element enemyElement)
        {
            EnemySpeed = enemySpeed;
            EnemyHp = enemyHp;
            GoldDrop = goldDrop;
            EnemyElement = enemyElement;
        }

        // protected abstract void EnemySkill(); //TODO: Later if want to add enemy skill
        
        
    }
}