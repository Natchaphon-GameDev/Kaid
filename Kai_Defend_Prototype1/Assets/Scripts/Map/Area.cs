using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Map
{
    public class Area : MonoBehaviour
    {
        // public static Area Instance { get; private set; }
        // [SerializeField] private Color hoverColor;
        // [SerializeField] private Color startColor;
        // [SerializeField] private Color MoreThanHeroLimitColor;
        public static float CurrentHeroAmount { get; private set; }
        public static float CurrentSubHeroAmount { get; private set; }

        [HideInInspector]
        public GameObject hero;

        [HideInInspector] 
        public HeroBluePrint HeroBluePrint;

        private bool moreThanHeroLimit => CurrentHeroAmount >= BuildManager.Instance.HeroLimit;
        private bool moreThanSubHeroLimit => CurrentSubHeroAmount >= BuildManager.Instance.SubHeroLimit;
        
        private void Start()
        {
            CurrentHeroAmount = 0;
            CurrentSubHeroAmount = 0;
        }

        public Vector3 GetPosition()
        {
            return transform.position + new Vector3(0f, -0.1f, 0f);
            //the difference in the height of the stage when play click to place hero
        }

        private Vector3 GetEvoPosition()
        {
            return transform.position + new Vector3(0f, +0.2f, 0f);
        }

        private void BuildHero(HeroBluePrint blueprint)
        {
            if (blueprint.IsBuilt)
            {
                return;
            }
            
            if (moreThanHeroLimit)
            {
                if (blueprint.IsHero)
                {
                    return;
                }
            }
            else if (moreThanSubHeroLimit)
            {
                if (!blueprint.IsHero)
                {
                    return;
                }
            }
            
            if (moreThanHeroLimit && moreThanSubHeroLimit)
            {
                return;
            }
            
            if (blueprint.IsHero)
            {
                CurrentHeroAmount++;
            }
            else
            {
                CurrentSubHeroAmount++;
            }

            HeroBluePrint = blueprint;
            
            var _hero = Instantiate(blueprint.Prefab, GetPosition(), Quaternion.identity);
            hero = _hero;
            ChainSkill.Instance.AddHeroToField(blueprint);
            blueprint.IsBuilt = true;
        }

        public void UpgradeHero()
        {
            MoneyManager.Instance.DeleteMoney(HeroBluePrint.UpgradeCost);
            HeroBluePrint.Level++;
            
            if (HeroBluePrint.AttackType == AttackType.Laser)
            {
                HeroBluePrint.DamageOverTime += InventoryManager.Instance.DmgOverTime;
            }
            else
            {
                HeroBluePrint.AttackDamage += InventoryManager.Instance.AtkDamage;
                HeroBluePrint.AttackSpeed += InventoryManager.Instance.AtkSpeed;
            }
            
            HeroBluePrint.HeroCostAmount += HeroBluePrint.UpgradeCost;
            GameManager.Instance.UpdateHeroInfo();
        }

        public void EvoHero()
        {
            Destroy(hero);
            if (!HeroBluePrint.IsHero)
            {
                if (HeroBluePrint.AttackType == AttackType.Laser)
                {
                    HeroBluePrint.DamageOverTime += GameManager.Instance.UpgradeAttackDamagePreEvo;
                    HeroBluePrint.AttackRange += GameManager.Instance.UpgradeAttackRangePreEvo;
                }
                else
                {
                    HeroBluePrint.AttackDamage += GameManager.Instance.UpgradeAttackDamagePreEvo;
                    HeroBluePrint.AttackRange += GameManager.Instance.UpgradeAttackRangePreEvo;
                }
                
                var subHeroEvo = Instantiate(HeroBluePrint.EvoPrefab, GetEvoPosition(), Quaternion.identity);
                hero = subHeroEvo;
                HeroBluePrint.IsEvo = true;
                return;
            }
            
            if (HeroBluePrint.IsPreEvo)
            {
                if (HeroBluePrint.AttackType == AttackType.Laser)
                {
                    HeroBluePrint.DamageOverTime += GameManager.Instance.UpgradeAttackDamagePreEvo;
                    HeroBluePrint.AttackRange += GameManager.Instance.UpgradeAttackRangePreEvo;
                }
                else
                {
                    HeroBluePrint.AttackDamage += GameManager.Instance.UpgradeAttackDamagePreEvo;
                    HeroBluePrint.AttackRange += GameManager.Instance.UpgradeAttackRangePreEvo;
                }
                var heroEvo2 = Instantiate(HeroBluePrint.Evo2Prefab, GetEvoPosition(), Quaternion.identity);
                hero = heroEvo2;
                HeroBluePrint.IsEvo = true;
                return;
            }
            
            if (HeroBluePrint.AttackType == AttackType.Laser)
            {
                HeroBluePrint.DamageOverTime += GameManager.Instance.UpgradeAttackDamagePreEvo;
                HeroBluePrint.AttackRange += GameManager.Instance.UpgradeAttackRangePreEvo;
            }
            else
            {
                HeroBluePrint.AttackDamage += GameManager.Instance.UpgradeAttackDamagePreEvo;
                HeroBluePrint.AttackRange += GameManager.Instance.UpgradeAttackRangePreEvo;
            }
            var heroEvo1 = Instantiate(HeroBluePrint.EvoPrefab, GetEvoPosition(), Quaternion.identity);
            hero = heroEvo1;
            HeroBluePrint.IsPreEvo = true;
        }

        public void SellHero()
        {
            if (HeroBluePrint.IsHero)
            {
                MoneyManager.Instance.AddMoney(HeroBluePrint.GetSellAmount() + BuildManager.Instance.HeroSellCost);
                CurrentHeroAmount--;
                //Delete Evo Status
                if (HeroBluePrint.IsPreEvo)
                {
                    Debug.Log("- Evo Status H1");
                    if (HeroBluePrint.AttackType == AttackType.Laser)
                    {
                        HeroBluePrint.DamageOverTime -= GameManager.Instance.UpgradeAttackDamagePreEvo;
                        HeroBluePrint.AttackRange -= GameManager.Instance.UpgradeAttackRangePreEvo;
                    }
                    else
                    {
                        HeroBluePrint.AttackDamage -= GameManager.Instance.UpgradeAttackDamagePreEvo;
                        HeroBluePrint.AttackRange -= GameManager.Instance.UpgradeAttackRangePreEvo;
                    }
                    HeroBluePrint.IsPreEvo = false;
                }
                if (HeroBluePrint.IsEvo)
                {
                    Debug.Log("- Evo Status H2");
                    if (HeroBluePrint.AttackType == AttackType.Laser)
                    {
                        HeroBluePrint.DamageOverTime -= GameManager.Instance.UpgradeAttackDamagePreEvo;
                        HeroBluePrint.AttackRange -= GameManager.Instance.UpgradeAttackRangePreEvo;
                    }
                    else
                    {
                        HeroBluePrint.AttackDamage -= GameManager.Instance.UpgradeAttackDamagePreEvo;
                        HeroBluePrint.AttackRange -= GameManager.Instance.UpgradeAttackRangePreEvo;
                    }
                    HeroBluePrint.IsEvo = false;
                }
            }
            else
            {
                MoneyManager.Instance.AddMoney(BuildManager.Instance.SubHeroSellCost);
                CurrentSubHeroAmount--;
                //Delete Evo Status
                if (HeroBluePrint.IsEvo)
                {
                    Debug.Log("- Evo Status SH");
                    if (HeroBluePrint.AttackType == AttackType.Laser)
                    {
                        HeroBluePrint.DamageOverTime -= GameManager.Instance.UpgradeAttackDamagePreEvo;
                        HeroBluePrint.AttackRange -= GameManager.Instance.UpgradeAttackRangePreEvo;
                    }
                    else
                    {
                        HeroBluePrint.AttackDamage -= GameManager.Instance.UpgradeAttackDamagePreEvo;
                        HeroBluePrint.AttackRange -= GameManager.Instance.UpgradeAttackRangePreEvo;
                    }
                    HeroBluePrint.IsEvo = false;
                }
            }

            ChainSkill.Instance.RemoveHeroInField(HeroBluePrint);
            
            HeroBluePrint.IsBuilt = !true;
            HeroBluePrint.HeroCostAmount = 0;
            HeroBluePrint.Level = 0;
            Destroy(hero);
            HeroBluePrint = null;
            var debugNull = InventoryManager.Instance.HeroTeam.FirstOrDefault(hero => hero.HeroName == HeroName.Null);

            BuildManager.Instance.heroToBuild = debugNull; //I don't know why but it can debug many null ref for bullet
        }

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (hero != null && InventoryManager.Instance.IsBuildMode == false)
            {
                BuildManager.Instance.SelectHero(this);
                return;
            }
            
            if (hero != null)
            {
                return;
            }
            
            if (!BuildManager.Instance.CanBuild)
            {
                return;
            }


            BuildHero(BuildManager.Instance.GetHeroToBuild());

            if (InventoryManager.Instance.UsedCardTemp != null)
            {
                Destroy(InventoryManager.Instance.UsedCardTemp.gameObject);
            }

            // foreach (var card in InventoryManager.Instance.CardsInventory)
            // {
            //     if (card == null)
            //     {
            //         continue;
            //     }
            //     card.interactable = true;
            // }
            
            UiManager.Instance.blockUI.gameObject.SetActive(false);
            AnimationManager.Instance.MapDown();
            InventoryManager.Instance.IsBuildMode = false;
            
            InventoryManager.Instance.UsedCardTempPanel.gameObject.SetActive(false);
            
            UiManager.Instance.UpdateCardInventory();
            
            UiManager.Instance.RandomScrollButtonArea.gameObject.SetActive(!false);
            InventoryManager.Instance.InventoryButton.gameObject.SetActive(true);
        }
    }
}