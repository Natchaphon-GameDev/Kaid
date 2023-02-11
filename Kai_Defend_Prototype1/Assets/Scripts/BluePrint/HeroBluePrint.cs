using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    [System.Serializable]
    public class HeroBluePrint
    {
        [HideInInspector]
        public string Name;
        [Header("Hero Name")]
        public HeroName HeroName;
        
        [Header("Attack Type")]
        public AttackType AttackType;
        
        [Header("Prefab")]
        public GameObject Prefab;
        public GameObject EvoPrefab;
        public GameObject Evo2Prefab;
        
        [Header("Element")]
        public Element ElementType;

        [Header("CheckList")]
        public bool IsHero;
        [HideInInspector]
        public bool IsBuilt;
        [HideInInspector]
        public bool IsEvo;
        [HideInInspector]
        public bool IsPreEvo;

        [Header("Laser Tower")]
        public float DamageOverTime; 
        public float SlowPercent = 0.3f;
        
        [Header("Status")]
        public float BulletSpeed = 20f;
        public float AttackDamage;
        public float AttackSpeed;
        public float AttackRange;
        public float AoeRange = 0f;
        public int UpgradeCost = 100;

        [HideInInspector] 
        public int Level;

        [HideInInspector]
        public int HeroCostAmount;
        
        public int GetSellAmount()
        {
            return (HeroCostAmount / 2);
        }
    }
    
    public enum HeroName
    {
        F81_Hermes,
        F13_Colorado,
        W75_Valiant,
        W410_Odin,
        TEP58_Interpid,
        P32_Enterprise,
        Z1_Hornet,
        Z23_Glorious,
        Z25_Terror,
        Z46_Genesis,
        U73_Aurora,
        U556_Radford,
        U110_Sendai,
        U101_Jupiter,
        I168_Memphis,
        I13_Centaur,
        I58_Essex,
        I26_Albacore,
        Null
    }

    public enum Element
    {
        Fire,
        Water,
        Plant,
        Null
    }
    
    public enum AttackType
    {
        MachineGun,
        Missile,
        Laser
    }
}

