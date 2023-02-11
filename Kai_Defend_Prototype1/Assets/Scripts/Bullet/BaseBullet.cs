using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemy;
using Hero;
using Manager;
using Map;
using UnityEngine;

namespace Bullet
{
    public class BaseBullet : MonoBehaviour
    {
        private Transform targetEnemy;

        protected float BulletSpeed;
        protected float AttackDamage;
        protected float AttackRange;
        [SerializeField] protected GameObject hitEffect;
        
        protected HeroBluePrint hero;

        protected void GetStatus()
        {
            AttackDamage = hero.AttackDamage;
            AttackRange = hero.AttackRange;
            BulletSpeed = hero.BulletSpeed;
        }

        protected HeroBluePrint GetHeroBluePrint(HeroName heroName)
        {
            return InventoryManager.Instance.HeroTeam.FirstOrDefault(hero => hero.HeroName == heroName);
        }

        protected void MoveToTarget()
        {
            
            if (targetEnemy == null)
            {
                Destroy(gameObject);
                return;
            }

            var direction = targetEnemy.position - transform.position;
            var fixDistanceOnFame = hero.BulletSpeed * Time.deltaTime;

            if (direction.magnitude <= fixDistanceOnFame)
            {
                EnemyHit();
                return;
            }

            transform.Translate(direction.normalized * fixDistanceOnFame, Space.World);
            transform.LookAt(targetEnemy);
        }

        public void FindEnemy(Transform target)
        {
            targetEnemy = target;
        }

        private void EnemyHit()
        {
            if (hero.AoeRange > 0)
            {
                IsAoe();
            }
            else
            {
                Damage(targetEnemy);
            }

            Instantiate(hitEffect, transform.position, transform.rotation);

            Destroy(gameObject);
        }

        private void IsAoe()
        {
            var colliders = Physics.OverlapSphere(transform.position, hero.AoeRange);

            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    Damage(collider.transform);
                }
            }
        }

        private void Damage(Component target)
        {
            PopUpDamage.CheckLaserType = false;
            if (target.GetComponent<Colubted>() != null) //Plant
            {
                var enemy = target.GetComponent<Colubted>();
                switch (hero.ElementType)
                {
                    case Element.Fire:
                        enemy.TakeDamage(hero.AttackDamage + ChainSkill.Instance.elementChain.FireElementDamage);
                        break;
                    case Element.Water:
                        enemy.TakeDamage(hero.AttackDamage - ChainSkill.Instance.elementChain.LoseElementDamage);
                        break;
                    case Element.Plant:
                        enemy.TakeDamage(hero.AttackDamage);
                        break;
                }
            }
            else if (target.GetComponent<Embio>() != null) //Fire
            {
                var enemy = target.GetComponent<Embio>();
                switch (hero.ElementType)
                {
                    case Element.Water:
                        enemy.TakeDamage(hero.AttackDamage + ChainSkill.Instance.elementChain.WaterElementDamage);
                        break;
                    case Element.Plant:
                        enemy.TakeDamage(hero.AttackDamage - ChainSkill.Instance.elementChain.LoseElementDamage);
                        break;
                    case Element.Fire:
                        enemy.TakeDamage(hero.AttackDamage);
                        break;
                }
            }
            else if (target.GetComponent<Eclipseside>() != null) //Water
            {
                var enemy = target.GetComponent<Eclipseside>();
                switch (hero.ElementType)
                {
                    case Element.Plant:
                        enemy.TakeDamage(hero.AttackDamage + ChainSkill.Instance.elementChain.PlantElementDamage);
                        break;
                    case Element.Fire:
                        enemy.TakeDamage(hero.AttackDamage - ChainSkill.Instance.elementChain.LoseElementDamage);
                        break;
                    case Element.Water:
                        enemy.TakeDamage(hero.AttackDamage);
                        break;
                }
            }
            else if (target.GetComponent<CrossDive>() != null) //Plant
            {
                var enemy = target.GetComponent<CrossDive>();
                switch (hero.ElementType)
                {
                    case Element.Fire:
                        enemy.TakeDamage(hero.AttackDamage + ChainSkill.Instance.elementChain.FireElementDamage);
                        break;
                    case Element.Water:
                        enemy.TakeDamage(hero.AttackDamage - ChainSkill.Instance.elementChain.LoseElementDamage);
                        break;
                    case Element.Plant:
                        enemy.TakeDamage(hero.AttackDamage);
                        break;
                }
            }
            else if (target.GetComponent<Shiro>() != null) //Fire
            {
                var enemy = target.GetComponent<Shiro>();
                switch (hero.ElementType)
                {
                    case Element.Water:
                        enemy.TakeDamage(hero.AttackDamage + ChainSkill.Instance.elementChain.WaterElementDamage);
                        break;
                    case Element.Plant:
                        enemy.TakeDamage(hero.AttackDamage - ChainSkill.Instance.elementChain.LoseElementDamage);
                        break;
                    case Element.Fire:
                        enemy.TakeDamage(hero.AttackDamage);
                        break;
                }
            }
            else if (target.GetComponent<Kana>() != null) //Water
            {
                var enemy = target.GetComponent<Kana>();
                switch (hero.ElementType)
                {
                    case Element.Plant:
                        enemy.TakeDamage(hero.AttackDamage + ChainSkill.Instance.elementChain.PlantElementDamage);
                        break;
                    case Element.Fire:
                        enemy.TakeDamage(hero.AttackDamage - ChainSkill.Instance.elementChain.LoseElementDamage);
                        break;
                    case Element.Water:
                        enemy.TakeDamage(hero.AttackDamage);
                        break;
                }
            }
            else if (target.GetComponent<Chest>() != null)
            {
                target.GetComponent<Chest>().TakeDamage(hero.AttackDamage);
            }
        }

        protected void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, hero.AoeRange);
        }
    }
}