using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Manager;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CardBluePrint
{
    [HideInInspector]
    public string HeroName;
    public HeroName CardName;
    public Button Card;
    public List<Button> CardToEvo;
    public int CardAmount;
    public bool IsHeroCard;
}
