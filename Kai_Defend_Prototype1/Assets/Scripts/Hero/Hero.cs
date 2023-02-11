using System;
using System.Collections;
using System.Collections.Generic;
using Bullet;
using Enemy;
using Manager;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Hero
{
    public class Hero : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private Transform heroRotatePoint;
        [SerializeField] private float heroTurnSpeed = 10;
        [SerializeField] private string enemyTag = "Enemy";
        public Transform Firepoint;
        
        [Header("Laser Tower")] 
        public bool UseLaser;
        public LineRenderer RenderLaser;
        public ParticleSystem LaserEffect;
        public Light LaserLight;
        
        [Header("Bullet Tower")]
        public GameObject bulletPrefab;

        private Transform targetEnemy;

        [SerializeField]private HeroBluePrint hero;

        private Transform attackPoint;

        private float attackCountdown;

        private Colubted m_Colubted;
        private Embio m_Embio;
        private Eclipseside m_Eclipseside;
        private CrossDive m_CrossDive;
        private Shiro m_Shiro;
        private Kana m_Kana;
        private Chest m_Chest;
        
        [Header("Item Effect")]
        public GameObject cookieAura;
        public GameObject candyAura;
        
        private Transform _transform;

        private Hero checkDestroy;
        private void Awake()
        {
            hero = BuildManager.Instance.heroToBuild;
        }

        private void Start()
        {
            _transform = GetComponent<Transform>();
            InvokeRepeating("UpdateTarget", 0f, 0.5f);
            ItemManager.Instance.OnCandyUsed += CandyEffect;
            ItemManager.Instance.OnCookieUsed += CookieEffect;
        }

        private void Update()
        {
            if (targetEnemy == null)
            {
                if (UseLaser)
                {
                    if (RenderLaser.enabled)
                    {
                        RenderLaser.enabled = false;
                        LaserLight.enabled = false;
                        LaserEffect.Stop();
                    }
                }
                return;
            }

            LockOnTarget();

            if (UseLaser)
            {
                Laser();
            }
            else
            {
                if (attackCountdown <= 0f)
                {
                    Attack();
                    attackCountdown = 1f / hero.AttackSpeed;
                }
                attackCountdown -= Time.deltaTime;
            }
        }
        
        private void CandyEffect()
        {
            Instantiate(candyAura, _transform.position, _transform.rotation);
        }
        
        private void CookieEffect()
        {
            Instantiate(cookieAura, _transform.position,_transform.rotation);
        }

        private void LockOnTarget()
        {
            var directionToTarget = targetEnemy.position - transform.position;
            var lookToEnemy = Quaternion.LookRotation(directionToTarget);
            var rotation = Quaternion.Lerp(heroRotatePoint.rotation, lookToEnemy, Time.deltaTime * heroTurnSpeed).eulerAngles;
            heroRotatePoint.rotation = Quaternion.Euler (0f, rotation.y ,0f);
        }

        private void Laser()
        {
            GetEnemyElement();

            if (!RenderLaser.enabled)
            {
                RenderLaser.enabled = true;
                LaserLight.enabled = true;
                LaserEffect.Play();
            }
            RenderLaser.SetPosition(0, Firepoint.position);
            RenderLaser.SetPosition(1,targetEnemy.position);

            var direction = Firepoint.position - targetEnemy.position;

            LaserEffect.transform.position = targetEnemy.position + direction.normalized * 0.3f;

            LaserEffect.transform.rotation = Quaternion.LookRotation(direction);
        }

        private void GetEnemyElement()
        {
            PopUpDamage.CheckLaserType = true;
            if (hero.ElementType == Element.Water)
            {
                if (m_Embio != null) //FIRE
                {
                    m_Embio.TakeDamage((hero.DamageOverTime + (ChainSkill.Instance.elementChain.WaterElementDamage)) * Time.deltaTime);
                    m_Embio.Slow(hero.SlowPercent);
                }
                else if (m_Shiro != null) //FIRE
                {
                    m_Shiro.TakeDamage((hero.DamageOverTime + (ChainSkill.Instance.elementChain.WaterElementDamage)) * Time.deltaTime);
                    m_Shiro.Slow(hero.SlowPercent);
                }
                else if (m_Colubted != null) //Plant
                {
                    m_Colubted.TakeDamage((hero.DamageOverTime - (ChainSkill.Instance.elementChain.LoseElementDamage)) * Time.deltaTime);
                    m_Colubted.Slow(hero.SlowPercent);
                }
                else if (m_CrossDive != null) //Plant
                {
                    m_CrossDive.TakeDamage((hero.DamageOverTime - (ChainSkill.Instance.elementChain.LoseElementDamage)) * Time.deltaTime);
                    m_CrossDive.Slow(hero.SlowPercent);
                }
                else if (m_Eclipseside != null) //Water
                {
                    m_Eclipseside.TakeDamage(hero.DamageOverTime * Time.deltaTime);
                    m_Eclipseside.Slow(hero.SlowPercent);
                }
                else if (m_Kana != null) //Water
                {
                    m_Kana.TakeDamage(hero.DamageOverTime * Time.deltaTime);
                    m_Kana.Slow(hero.SlowPercent);
                }
                else if (m_Chest != null)
                {
                    m_Chest.TakeDamage(hero.DamageOverTime * Time.deltaTime);
                    m_Chest.Slow(hero.SlowPercent);
                }
            }
            else if (hero.ElementType == Element.Fire) 
            {
                if (m_Colubted != null) //Plant
                {
                    m_Colubted.TakeDamage((hero.DamageOverTime + (ChainSkill.Instance.elementChain.FireElementDamage)) * Time.deltaTime);
                    m_Colubted.Slow(hero.SlowPercent);
                }
                else if (m_CrossDive != null) //Plant
                {
                    m_CrossDive.TakeDamage((hero.DamageOverTime + (ChainSkill.Instance.elementChain.FireElementDamage)) * Time.deltaTime);
                    m_CrossDive.Slow(hero.SlowPercent);
                }
                else if (m_Eclipseside != null)  //Water
                {
                    m_Eclipseside.TakeDamage((hero.DamageOverTime - (ChainSkill.Instance.elementChain.LoseElementDamage)) * Time.deltaTime);
                    m_Eclipseside.Slow(hero.SlowPercent);
                }
                else if (m_Kana != null)  //Water
                {
                    m_Kana.TakeDamage((hero.DamageOverTime - (ChainSkill.Instance.elementChain.LoseElementDamage)) * Time.deltaTime);
                    m_Kana.Slow(hero.SlowPercent);
                }
                else  if (m_Embio != null) //Fire
                {
                    m_Embio.TakeDamage(hero.DamageOverTime * Time.deltaTime);
                    m_Embio.Slow(hero.SlowPercent);
                }
                else if (m_Shiro != null) //Fire
                {
                    m_Shiro.TakeDamage(hero.DamageOverTime * Time.deltaTime);
                    m_Shiro.Slow(hero.SlowPercent);

                }
                else if (m_Chest != null)
                {
                    m_Chest.TakeDamage(hero.DamageOverTime * Time.deltaTime);
                    m_Chest.Slow(hero.SlowPercent);
                }
            }
            else if (hero.ElementType == Element.Plant)
            {
                if (m_Eclipseside != null) //Water
                {
                    m_Eclipseside.TakeDamage((hero.DamageOverTime + (ChainSkill.Instance.elementChain.PlantElementDamage)) * Time.deltaTime);
                    m_Eclipseside.Slow(hero.SlowPercent);
                }
                else if (m_Kana != null) //Water
                {
                    m_Kana.TakeDamage((hero.DamageOverTime + (ChainSkill.Instance.elementChain.PlantElementDamage)) * Time.deltaTime);
                    m_Kana.Slow(hero.SlowPercent);
                }
                else if (m_Embio != null) //Fire
                {
                    m_Embio.TakeDamage((hero.DamageOverTime - (ChainSkill.Instance.elementChain.LoseElementDamage)) * Time.deltaTime);
                    m_Embio.Slow(hero.SlowPercent);
                }
                else if (m_Shiro != null) //Fire
                {
                    m_Shiro.TakeDamage((hero.DamageOverTime - (ChainSkill.Instance.elementChain.LoseElementDamage)) * Time.deltaTime);
                    m_Shiro.Slow(hero.SlowPercent);

                }
                else if (m_Colubted != null) //Plant
                {
                    m_Colubted.TakeDamage(hero.DamageOverTime * Time.deltaTime);
                    m_Colubted.Slow(hero.SlowPercent);
                }
                else if (m_CrossDive != null) //Plant
                {
                    m_CrossDive.TakeDamage(hero.DamageOverTime * Time.deltaTime);
                    m_CrossDive.Slow(hero.SlowPercent);
                }
                else if (m_Chest != null)
                {
                    m_Chest.TakeDamage(hero.DamageOverTime * Time.deltaTime);
                    m_Chest.Slow(hero.SlowPercent);
                }
            }
        }

        private void Attack()
        {
            var bulletGameObject= Instantiate(bulletPrefab, Firepoint.position, Firepoint.rotation);
            var bullet = bulletGameObject.GetComponent<BaseBullet>();

            if (bullet != null)
            {
                bullet.FindEnemy(targetEnemy);
            }
        }

        private void UpdateTarget()
        {
            var enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            var shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach (var enemy in enemies)
            {
                var distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }

            if (nearestEnemy != null && shortestDistance <= hero.AttackRange)
            {
                targetEnemy = nearestEnemy.transform;
                if (nearestEnemy.GetComponent<Colubted>() != null)
                {
                    m_Colubted = nearestEnemy.GetComponent<Colubted>();
                }
                else if (nearestEnemy.GetComponent<Embio>() != null)
                {
                    m_Embio = nearestEnemy.GetComponent<Embio>();
                }
                else if (nearestEnemy.GetComponent<Eclipseside>() != null)
                {
                    m_Eclipseside = nearestEnemy.GetComponent<Eclipseside>();
                }
                else if (nearestEnemy.GetComponent<CrossDive>() != null)
                {
                    m_CrossDive = nearestEnemy.GetComponent<CrossDive>();
                }
                else if (nearestEnemy.GetComponent<Shiro>() != null)
                {
                    m_Shiro = nearestEnemy.GetComponent<Shiro>();
                }
                else if (nearestEnemy.GetComponent<Kana>() != null)
                {
                    m_Kana = nearestEnemy.GetComponent<Kana>();
                }
                else if (nearestEnemy.GetComponent<Chest>() != null)
                {
                    m_Chest = nearestEnemy.GetComponent<Chest>();
                }
            }
            else
            {
                //Debug
                targetEnemy = null;
                m_Colubted = null;
                m_Embio = null;
                m_Eclipseside = null;
                m_CrossDive = null;
                m_Shiro = null;
                m_Kana = null;
                m_Chest = null;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, hero.AttackRange);
        }
    }
}

