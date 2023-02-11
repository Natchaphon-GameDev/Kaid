using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Manager;
using Map;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroUI : MonoBehaviour
{
    [SerializeField] private GameObject heroUI;
    private Area target;
    
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI sellCostText;
    [SerializeField] private TextMeshProUGUI heroNameText;
    [SerializeField] private TextMeshProUGUI subHeroNameText;
    [SerializeField] private TextMeshProUGUI evoCostText;
    [SerializeField] private TextMeshProUGUI upgradeButtonText;
    
    [Header("Upgrade Setup")]
    [SerializeField] private RectTransform levelPanel;
    [SerializeField] private Slider upgradeSlider;
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI heroLevelText;
    
    [Header("Button")]
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button evoButton;
    
    [Header("Circle Range Image")]
    [SerializeField] private RectTransform attackRangeImg;
    
    [Header("Evo Effect")]
    [SerializeField] private GameObject evoEffect;
    
    

    private Rect spriteRect;
    private int tempLevel;

    private void Awake()
    {
        HideHeroUI();
    }

    public void SetTarget(Area _target)
    {
        levelPanel.gameObject.SetActive(!true);
        upgradeSlider.SetValueWithoutNotify(0);

        target = _target;
        BuildManager.Instance.heroToBuild = target.HeroBluePrint; //for Update Hero Status before Evo
        ShowAttackRange();

        var getHeroCard = InventoryManager.Instance.Cards.Find(x => x.CardName == target.HeroBluePrint.HeroName);
        CheckHeroType();
        transform.position = target.GetPosition();
        upgradeCostText.text = "0 G";
        if (target.HeroBluePrint.IsHero)
        {
            sellCostText.text = $"{target.HeroBluePrint.GetSellAmount() + BuildManager.Instance.HeroSellCost} G"; 
            heroLevelText.text = $"Lv.{target.HeroBluePrint.Level}";
            heroNameText.text = $"{target.HeroBluePrint.HeroName.ToString()}";
            subHeroNameText.gameObject.SetActive(false);
            heroLevelText.gameObject.SetActive(true);
            heroNameText.gameObject.SetActive(true);
        }
        else
        {
            sellCostText.text = $"{BuildManager.Instance.SubHeroSellCost} G";
            subHeroNameText.text = $"{target.HeroBluePrint.HeroName.ToString()}";
            heroLevelText.gameObject.SetActive(false);
            heroNameText.gameObject.SetActive(false);
            subHeroNameText.gameObject.SetActive(true);
        }

        if (!target.HeroBluePrint.IsEvo)
        {
            evoCostText.text = $"{getHeroCard.CardAmount} / 3 ea.";
            evoButton.interactable = true;
        }
        else
        {
            evoCostText.text = "Max Evo!";
            evoButton.interactable = !true;
        }

        if (target.HeroBluePrint.Level >= 50)
        {
            upgradeButtonText.text = "Max Upgrade!";
            upgradeButton.interactable = false;
        }
        else
        {
            upgradeButtonText.text = $"Upgrade {target.HeroBluePrint.UpgradeCost}$";
            upgradeButton.interactable = !false;
        }

        heroUI.SetActive(true);
    }

    public void HideHeroUI()
    {
        levelPanel.gameObject.SetActive(!true);
        heroUI.SetActive(false);
    }
    
    private IEnumerator ChangeButtonColor()
    {
       upgradeButton.image.color = Color.red;
       yield return new WaitForSeconds (0.05f);
       upgradeButton.image.color = Color.white;
    }
    
    public void Upgrade()
    {
        if (MoneyManager.Instance.Money < target.HeroBluePrint.UpgradeCost)
        {
            UiManager.Instance.SetTextNotification("Not Enough\nMoney");
            UiManager.Instance.GetNotification();
            StartCoroutine(ChangeButtonColor());
            return;
        }
        levelPanel.gameObject.SetActive(true);
        levelText.text = $"Lv. {target.HeroBluePrint.Level + upgradeSlider.value}";
    }

    public void UpdateSlider()
    {
        UpdateLevel();
        upgradeSlider.maxValue = tempLevel;
        levelText.text = $"Lv. {target.HeroBluePrint.Level + upgradeSlider.value}";
        upgradeCostText.text = $"{target.HeroBluePrint.UpgradeCost * upgradeSlider.value} $";
    }

    public void UpgradeHero()
    {
        for (var i = 0; i < upgradeSlider.value; i++)
        {
            target.UpgradeHero();
        }
        levelPanel.gameObject.SetActive(!true);
        heroLevelText.text = $"Lv. {target.HeroBluePrint.Level}";
        upgradeSlider.SetValueWithoutNotify(0);
        sellCostText.text = $"{target.HeroBluePrint.GetSellAmount() + BuildManager.Instance.HeroSellCost} $";
        upgradeCostText.text = "0 $";
        BuildManager.Instance.DeselectHero();
    }

    private void UpdateLevel()
    {
        var temp = MoneyManager.Instance.Money;
        tempLevel = 0;
        while (temp >= target.HeroBluePrint.UpgradeCost)
        {
            //Break Lv 50
            if ((tempLevel + target.HeroBluePrint.Level) >= 50 || target.HeroBluePrint.Level >= 50)
            {
                return;
            }
            
            tempLevel++;
            temp -= target.HeroBluePrint.UpgradeCost;
        }
    }

    public void EvoHero()
    {
        int listCount;
        var checkloop = 0;
        var getHeroCard = InventoryManager.Instance.Cards.Find(x => x.CardName == target.HeroBluePrint.HeroName);
        if (getHeroCard.CardAmount >= 3)
        {
            getHeroCard.CardAmount -= 3;

            listCount = getHeroCard.CardToEvo.Count;
            
            for (var i = 0; i < listCount; i++)
            {
                if (getHeroCard.CardToEvo[i] == null)
                {
                    getHeroCard.CardToEvo.Remove(getHeroCard.CardToEvo[i]);
                    listCount--;
                    i--;
                }
                else
                {
                    Destroy(getHeroCard.CardToEvo[i].gameObject);
                    getHeroCard.CardToEvo.Remove(getHeroCard.CardToEvo[i]);
                    listCount--;
                    i--;
                    checkloop++;
                }

                if (checkloop >= 3)
                {
                    target.EvoHero();
                    BuildManager.Instance.DeselectHero();
                    //Update UI
                    StartCoroutine(WaitForUpdateUi());
                    Instantiate(evoEffect,target.transform.position + Vector3.up,target.transform.rotation);
                    return;
                }
            }
        }
    }

    private IEnumerator WaitForUpdateUi()
    {
        //I dont Know why but its work for update inventory card ui 
        yield return new WaitForSeconds(0.1f);
        UiManager.Instance.UpdateCardInventory();
    }

    public void SellHero()
    {
        //Delete Upgrade Status
        for (var i = 0; i < target.HeroBluePrint.Level; i++)
        {
            if (target.HeroBluePrint.AttackType == AttackType.Laser)
            {
                target.HeroBluePrint.DamageOverTime -= InventoryManager.Instance.DmgOverTime;
            }
            else
            {
                target.HeroBluePrint.AttackDamage -= InventoryManager.Instance.AtkDamage;
                target.HeroBluePrint.AttackSpeed -= InventoryManager.Instance.AtkSpeed;
            }
        }
        
        target.SellHero();
        BuildManager.Instance.DeselectHero();
    }

    private void CheckHeroType()
    {
        if (target.HeroBluePrint.IsHero == false)
        {
            upgradeButton.gameObject.SetActive(!true);
        }
        else
        {
            upgradeButton.gameObject.SetActive(true);
        }
    }
    
    private void ShowAttackRange()
    {
        attackRangeImg.localScale =
            new Vector3(target.HeroBluePrint.AttackRange / 2f, target.HeroBluePrint.AttackRange / 2f,0); // r=D/2 
    }




}
