using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Hero;
using JetBrains.Annotations;
using Map;
using UnityEngine;

namespace Manager
{
    public class BuildManager : MonoBehaviour
    {
        public static BuildManager Instance { get; private set; }
        public HeroBluePrint heroToBuild;
        
        [Header("Build Setting")]
        public float HeroLimit = 2;
        public float SubHeroLimit = 4;
        public int HeroSellCost = 50;
        public int SubHeroSellCost = 25;

        private Area selectedHero;
        
        [SerializeField] private HeroUI heroUI;

        public bool EnableSelectHero;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            EnableSelectHero = false;
        }

        public bool CanBuild => heroToBuild.Prefab != null;
        public bool IsHeroBuilt => heroToBuild.IsBuilt;
        

        public void SelectHero(Area hero)
        {
            if (!EnableSelectHero)
            {
                return;
            }
            
            if (selectedHero == hero)
            {
                DeselectHero();
                return;
            }
            
            selectedHero = hero;
            
            heroUI.SetTarget(hero);
            
            GameManager.Instance.UpdateHeroInfo();

        }

        public void DeselectHero()
        {
            foreach (var picture in WorldUIManager.Instance.HeroPictures)
            {
                picture.gameObject.SetActive(false);
            }
            WorldUIManager.Instance.UpdateHeroInfoText("");
            selectedHero = null;
            heroUI.HideHeroUI();
        }

        public void SetHero(HeroBluePrint hero)
        {
            heroToBuild = hero;
            DeselectHero();
        }

        public HeroBluePrint GetHeroToBuild()
        {
            return heroToBuild;
        }
    }
}

