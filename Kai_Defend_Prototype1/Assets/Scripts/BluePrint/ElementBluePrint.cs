using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ElementBluePrint
{
    [Header("6 Fire Status Boost")]
    public float Fire6_Add_BestFireDamage;
    public bool Fire6_Active;

    [Header("4 Fire Status Boost")]
    public float Fire4_Add_MoreFireDamage;
    public bool Fire4_Active;

    [Header("2 Fire Status Boost")]
    public float Fire2_Add_FireDamage;
    public bool Fire2_Active;
    
    [Header("6 Water Status Boost")]
    public float Water6_Add_Add_BestWaterDamage;
    public bool Water6_Active;

    [Header("4 Water Status Boost")]
    public float Water4_Add_MoreWaterDamage;
    public bool Water4_Active;
   
    [Header("2 Water Status Boost")]
    public float Water2_Add_WaterDamage;
    public bool Water2_Active;
    
    [Header("6 Plant Status Boost")]
    public float Plant6_Add_BestPlantDamage;
    public bool Plant6_Active;
    
    [Header("4 Plant Status Boost")]
    public float Plant4_Add_MorePlantDamage;
    public bool Plant4_Active;
   
    [Header("2 Plant Status Boost")]
    public float Plant2_Add_PlantDamage;
    public bool Plant2_Active;
    
    [Header("Element Setting")] 
    public float LoseElementDamage = 20;
    public float FireElementDamage = 20;
    public float WaterElementDamage = 20;
    public float PlantElementDamage = 20;
}
