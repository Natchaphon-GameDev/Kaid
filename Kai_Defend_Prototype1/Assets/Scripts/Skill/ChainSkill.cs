using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class ChainSkill : MonoBehaviour
{
    public static ChainSkill Instance { get; private set; }
    public List<SkillBluePrint> skillChains;
    public ElementBluePrint elementChain;

    public List<HeroBluePrint> HeroInField;//if Raider didn't say to change to private dont change okaY? it'll null ref
    
    public int  FireElementCount;
    public int  WaterElementCount;
    public int  PlantElementCount;

    //ValueChecker
    private int fireValueWatcher;
    private int waterValueWatcher;
    private int plantValueWatcher;

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
        GameManager.Instance.GameOver += GetCompGameOver;
    }

    private void GetCompGameOver()
    {
        var compTemp = skillChains.FindAll(x => x.IsActive);
        switch (compTemp.Count)
        {
            case 0 :
                UiManager.Instance.SetCompUsed("No Comp Used");
                break;
            case 1:
                UiManager.Instance.SetCompUsed($"-{compTemp[0].BuildName.ToString()}-"); 
                break;
            case 2:
                UiManager.Instance.SetCompUsed($"-{compTemp[0].BuildName.ToString()}-\n" +
                                               $"-{compTemp[1].BuildName.ToString()}-");
                break;
            case 3:
                UiManager.Instance.SetCompUsed($"-{compTemp[0].BuildName.ToString()}-\n" +
                                               $"-{compTemp[1].BuildName.ToString()}-\n" +
                                               $"-{compTemp[2].BuildName.ToString()}-");
                break;
        }
        
        //TODO LATER: If Update and in one game there can be more than 3 comp?
    }

    private void CheckSkillChainActive()
    {
        foreach (var skillChain in skillChains)
        {
            if (skillChain.IsActive && !skillChain.IsShowOnUI)
            {
                if (skillChain.IsActive && skillChain.IsShowOnUI)
                {
                    continue;
                }

                UiManager.Instance.SetTextNotification($"Chain Hero\n{skillChain.BuildName.ToString()}\nActivated! ");
                UiManager.Instance.GetNotification();
                SetBuff(skillChain);
                UiManager.Instance.SpawnChainSkillText(skillChain.BuildName.ToString()); //Generate all buff

                skillChain.IsShowOnUI = true;
            }
            else if (!skillChain.IsActive && skillChain.IsShowOnUI)
            {
                if (!skillChain.IsActive && !skillChain.IsShowOnUI)
                {
                    continue;
                }
                
                UiManager.Instance.SetTextNotification($"Chain Hero\n{skillChain.BuildName.ToString()}\nDisable! ");
                UiManager.Instance.GetNotification();
                RemoveBuff(skillChain);
                
                UiManager.Instance.RemoveChainSkillText(skillChain.BuildName.ToString()); //Remove all buff
                skillChain.IsShowOnUI = false;
            }
        }
    }

    #region Main event //Refactor next time
    
    
    public void AddHeroToField(HeroBluePrint blueprint)
    {
        HeroInField.Add(blueprint);

        foreach (var skillChain in skillChains)
        {
            FindBuild(HeroInField, skillChain, skillChain.HeroRequirements);
        }

        CheckSkillChainActive();
        FindElementBuild();
    }

    #endregion
    
    public void RemoveHeroInField(HeroBluePrint blueprint)
    {
        HeroInField.Remove(blueprint);
        foreach (var skillChain in skillChains)
        {
            FindBuild(HeroInField, skillChain, skillChain.HeroRequirements);
        }

        CheckSkillChainActive();
        FindElementBuild();
    }
    
    private void SetBuff(SkillBluePrint build)
    {
        foreach (var hero in InventoryManager.Instance.HeroTeam)
        {
                
                if (build.IsForMachine_Gun)
                {
                    if (hero.AttackType == AttackType.MachineGun)
                    {
                        hero.AttackDamage += build.AttackDamageBuff;
                        hero.AttackSpeed += build.AttackSpeedBuff;
                        hero.AttackRange += build.AttackRangeBuff;
                    }
                    continue;
                }
                if (build.IsForMissile)
                {
                    if (hero.AttackType == AttackType.Missile)
                    {
                        hero.AoeRange += build.AoeBuff;
                        hero.AttackSpeed += build.AttackSpeedBuff;
                        hero.AttackRange += build.AttackRangeBuff;
                    }
                    continue;
                }
                if (build.IsForLaser)
                {
                    if (hero.AttackType == AttackType.Laser)
                    {
                        hero.AttackDamage += build.AttackDamageBuff;
                        hero.SlowPercent += build.SlowBuff;
                    }
                    continue;
                }
                hero.AttackDamage += build.AttackDamageBuff;
                hero.AttackSpeed += build.AttackSpeedBuff;
                hero.AttackRange += build.AttackRangeBuff;
                elementChain.FireElementDamage += build.FireDamageBuff;
                elementChain.WaterElementDamage += build.WaterDamageBuff;
                elementChain.PlantElementDamage+= build.PlantDamageBuff;
        }
    }
    
    private void RemoveBuff(SkillBluePrint build)
    {
        foreach (var hero in InventoryManager.Instance.HeroTeam)
        {
            if (build.IsForMachine_Gun)
            {
                if (hero.AttackType == AttackType.MachineGun)
                {
                    hero.AttackDamage -= build.AttackDamageBuff;
                    hero.AttackSpeed -= build.AttackSpeedBuff;
                    hero.AttackRange -= build.AttackRangeBuff; 
                }
                continue;
            }
            if (build.IsForMissile)
            {
                if (hero.AttackType == AttackType.Missile)
                {
                    hero.AoeRange -= build.AoeBuff;
                    hero.AttackSpeed -= build.AttackSpeedBuff;
                    hero.AttackRange -= build.AttackRangeBuff;
                }
                continue;
            }
            if (build.IsForLaser)
            {
                if (hero.AttackType == AttackType.Laser)
                {
                    hero.AttackDamage -= build.AttackDamageBuff;
                    hero.SlowPercent -= build.SlowBuff;
                }
                continue;
            }
            
            hero.AttackDamage -= build.AttackDamageBuff;
            hero.AttackSpeed -= build.AttackSpeedBuff;
            hero.AttackRange -= build.AttackRangeBuff;
            elementChain.FireElementDamage -= build.FireDamageBuff;
            elementChain.WaterElementDamage -= build.WaterDamageBuff;
            elementChain.PlantElementDamage-= build.PlantDamageBuff;
        }
    }

    private void FindBuild (List<HeroBluePrint> heros, SkillBluePrint skill,List<HeroName> heroRequires)
    {
        if (heroRequires.Count == 2)
        {
            var hero1Detected = heros.Any(x => x.HeroName == heroRequires[0]);
            var hero2Detected = heros.Any(x => x.HeroName == heroRequires[1]);
            
            if (hero1Detected && hero2Detected)
            {
                skill.IsActive = true;
            }
            else
            {
                skill.IsActive = false;
            }
        }
        else if (heroRequires.Count == 3)
        {
            var hero1Detected = heros.Any(x => x.HeroName == heroRequires[0]);
            var hero2Detected = heros.Any(x => x.HeroName == heroRequires[1]);
            var hero3Detected = heros.Any(x => x.HeroName == heroRequires[2]);
            
            if (hero1Detected && hero2Detected && hero3Detected)
            {
                skill.IsActive = true;
            }
            else
            {
                skill.IsActive = false;
            }
        }
        else if (heroRequires.Count == 4)
        {
            var hero1Detected = heros.Any(x => x.HeroName == heroRequires[0]);
            var hero2Detected = heros.Any(x => x.HeroName == heroRequires[1]);
            var hero3Detected = heros.Any(x => x.HeroName == heroRequires[2]);
            var hero4Detected = heros.Any(x => x.HeroName == heroRequires[3]);

            if (hero1Detected && hero2Detected && hero3Detected && hero4Detected)
            {
                skill.IsActive = true;
            }
            else
            {
                skill.IsActive = false;
            }
        }
        
    }

    //Element
    private void FindElementBuild ()
    {
        var fireElement = HeroInField.FindAll(x => x.ElementType == Element.Fire);

        var waterElement = HeroInField.FindAll(x => x.ElementType == Element.Water);

        var plantElement = HeroInField.FindAll(x => x.ElementType == Element.Plant);

        fireValueWatcher = FireElementCount;
        waterValueWatcher = WaterElementCount;
        plantValueWatcher = PlantElementCount;
        
        FireElementCount = fireElement.Count;
        WaterElementCount = waterElement.Count;
        PlantElementCount = plantElement.Count;

        if (fireValueWatcher < FireElementCount)
        {
            UiManager.Instance.SpawnChainElement(0);
            fireValueWatcher = FireElementCount;
        }
        else if (fireValueWatcher > FireElementCount)
        {
            UiManager.Instance.RemoveChainElement(0);
            fireValueWatcher = FireElementCount;
        }
        
        if (waterValueWatcher < WaterElementCount)
        {
            UiManager.Instance.SpawnChainElement(1);
            waterValueWatcher = WaterElementCount;
        }
        else if (waterValueWatcher > WaterElementCount)
        {
            UiManager.Instance.RemoveChainElement(1);
            waterValueWatcher = WaterElementCount;
        }
        
        if (plantValueWatcher < PlantElementCount)
        {
            UiManager.Instance.SpawnChainElement(2);
            plantValueWatcher = PlantElementCount;
        }
        else if (plantValueWatcher > PlantElementCount)
        {
            UiManager.Instance.RemoveChainElement(2);
            plantValueWatcher = PlantElementCount;
        }

        #region Fire

         if (FireElementCount >= 2 && !elementChain.Fire2_Active)
        {
            elementChain.Fire2_Active = true;
            elementChain.FireElementDamage += elementChain.Fire2_Add_FireDamage;
            UiManager.Instance.SetTextNotification("Chain Element \n2 Fire Activate");
            UiManager.Instance.GetNotification();
        }
        else if (FireElementCount < 2 && elementChain.Fire2_Active)
        {
            elementChain.Fire2_Active = false;
            elementChain.FireElementDamage -= elementChain.Fire2_Add_FireDamage;
            UiManager.Instance.SetTextNotification("Chain Element \n2 Fire Disable");
            UiManager.Instance.GetNotification();
        }

        if (FireElementCount >= 4 && !elementChain.Fire4_Active)
        {
            elementChain.Fire4_Active = true;
            elementChain.FireElementDamage += elementChain.Fire4_Add_MoreFireDamage;
            UiManager.Instance.SetTextNotification("Chain Element \n4 Fire Activate");
            UiManager.Instance.GetNotification();
        }
        else if (FireElementCount < 4 && elementChain.Fire4_Active)
        {
            elementChain.Fire4_Active = false;
            elementChain.FireElementDamage -= elementChain.Fire4_Add_MoreFireDamage;
            UiManager.Instance.SetTextNotification("Chain Element \n4 Fire Disable");
            UiManager.Instance.GetNotification();
        }
        
        if (FireElementCount >= 6 && !elementChain.Fire6_Active)
        {
            elementChain.Fire6_Active = true;
            elementChain.FireElementDamage += elementChain.Fire6_Add_BestFireDamage;
            UiManager.Instance.SetTextNotification("Chain Element \n6 Fire Activate");
            UiManager.Instance.GetNotification();
        }
        else if (FireElementCount < 6 && elementChain.Fire6_Active)
        {
            elementChain.Fire6_Active = false;
            elementChain.FireElementDamage -= elementChain.Fire6_Add_BestFireDamage;
            UiManager.Instance.SetTextNotification("Chain Element \n6 Fire Disable");
            UiManager.Instance.GetNotification();
        }

        #endregion

        #region Water

         if (WaterElementCount >= 2 && !elementChain.Water2_Active)
        {
            elementChain.Water2_Active = true;
            elementChain.WaterElementDamage += elementChain.Water2_Add_WaterDamage;
            UiManager.Instance.SetTextNotification("Chain Element \n2 Water Activate");
            UiManager.Instance.GetNotification();
        }
        else if (WaterElementCount < 2 && elementChain.Water2_Active)
        {
            elementChain.Water2_Active = false;
            elementChain.WaterElementDamage -= elementChain.Water2_Add_WaterDamage;
            UiManager.Instance.SetTextNotification("Chain Element \n2 Water Disable");
            UiManager.Instance.GetNotification();
        }

        if (WaterElementCount >= 4 && !elementChain.Water4_Active)
        {
            elementChain.Water4_Active = true;
            elementChain.WaterElementDamage += elementChain.Water4_Add_MoreWaterDamage;
            UiManager.Instance.SetTextNotification("Chain Element \n4 Water Activate");
            UiManager.Instance.GetNotification();
        }
        else if (WaterElementCount < 4 && elementChain.Water4_Active)
        {
            elementChain.Water4_Active = false;
            elementChain.WaterElementDamage -= elementChain.Water4_Add_MoreWaterDamage;
            UiManager.Instance.SetTextNotification("Chain Element \n4 Water Disable");
            UiManager.Instance.GetNotification();
        }
        
        if (WaterElementCount >= 6 && !elementChain.Water6_Active)
        {
            elementChain.Water6_Active = true;
            elementChain.WaterElementDamage += elementChain.Water6_Add_Add_BestWaterDamage;
            UiManager.Instance.SetTextNotification("Chain Element \n6 Water Activate");
            UiManager.Instance.GetNotification();
        }
        else if (WaterElementCount < 6 && elementChain.Water6_Active)
        {
            elementChain.Water6_Active = false;
            elementChain.WaterElementDamage -= elementChain.Water6_Add_Add_BestWaterDamage;
            UiManager.Instance.SetTextNotification("Chain Element \n6 Water Disable");
            UiManager.Instance.GetNotification();
        }

        #endregion

        #region Plant

        if (PlantElementCount >= 2 && !elementChain.Plant2_Active)
        {
            elementChain.Plant2_Active = true;
            elementChain.PlantElementDamage += elementChain.Plant2_Add_PlantDamage;
            UiManager.Instance.SetTextNotification("Chain Element \n2 Plant Activate");
            UiManager.Instance.GetNotification();
        }
        else if (PlantElementCount < 2 && elementChain.Plant2_Active)
        {
            elementChain.Plant2_Active = false;
            elementChain.PlantElementDamage -= elementChain.Plant2_Add_PlantDamage;
            UiManager.Instance.SetTextNotification("Chain Element \n2 Plant Disable");
            UiManager.Instance.GetNotification();
        }

        if (PlantElementCount >= 4 && !elementChain.Plant4_Active)
        {
            elementChain.Plant4_Active = true;
            elementChain.PlantElementDamage += elementChain.Plant4_Add_MorePlantDamage;
            UiManager.Instance.SetTextNotification("Chain Element \n4 Plant Activate");
            UiManager.Instance.GetNotification();
        }
        else if (PlantElementCount < 4 && elementChain.Plant4_Active)
        {
            elementChain.Plant4_Active = false;
            elementChain.PlantElementDamage -= elementChain.Plant4_Add_MorePlantDamage;
            UiManager.Instance.SetTextNotification("Chain Element \n4 Plant Disable");
            UiManager.Instance.GetNotification();
        }
        
        if (PlantElementCount >= 6 && !elementChain.Plant6_Active)
        {
            elementChain.Plant6_Active = true;
            elementChain.PlantElementDamage += elementChain.Plant6_Add_BestPlantDamage;
            UiManager.Instance.SetTextNotification("Chain Element \n6 Plant Activate");
            UiManager.Instance.GetNotification();
        }
        else if (PlantElementCount < 6 && elementChain.Plant6_Active)
        {
            elementChain.Plant6_Active = false;
            elementChain.PlantElementDamage -= elementChain.Plant6_Add_BestPlantDamage;
            UiManager.Instance.SetTextNotification("Chain Element \n6 Plant Disable");
            UiManager.Instance.GetNotification();
        }

        #endregion
    }
}


