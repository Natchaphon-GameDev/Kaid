using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using TMPro;
using UnityEngine;

[System.Serializable]
public class SkillBluePrint
{
    // [HideInInspector]
    [SerializeField] private string name;
    
    public BuildName BuildName;
    
    [Header("HeroRequire")] 
    public List<HeroName> HeroRequirements;

    [Header("Status Boost")]
    public float AttackDamageBuff;
    public float AttackSpeedBuff;
    public float AttackRangeBuff;
    public float SlowBuff;
    public float AoeBuff;

    [Header("Element Boost")]
    public float FireDamageBuff;
    public float WaterDamageBuff;
    public float PlantDamageBuff;
    
    [Header("Buff Only AttackType")]
    public bool IsForMachine_Gun;
    public bool IsForMissile;
    public bool IsForLaser;

    [Header("CheckList")]
    public bool IsActive;
    public bool IsShowOnUI;

}

public enum BuildName
{
    RedCouple,
    WaterShot,
    TwoSlow,
    EarthShock,
    OneAndTwo,
    GreenLaser,
    Fanning,
    DoubleCouple,
    TwoEnough,
    TripleTake,
    TripleSlow,
    ThousandShot,
    TwoTeamBetter,
    Boombaya,
    PowerShot,
    WaitForIt,
    TeamUp
}