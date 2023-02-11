using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }
    private float attackBoostTemp;

    public int CookieTime = 30;
    public int CandyTime = 30;

    public event Action OnCandyUsed;
    public event Action OnCookieUsed;

    #region ItemFireWork

    [SerializeField] private List<float> damageTemp;
    [SerializeField] private List<float> speedTemp;

    [Header("Effect")] 
    [SerializeField] private GameObject firework;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void FireWork()
    {
        Instantiate(firework);
        var enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemys)
        {
            GameManager.Instance.EnemyCount--;
            if (enemy.GetComponent<Chest>() != null)
            {
                continue;
            }
            Destroy(enemy.gameObject);
        }
    }
    #endregion

    #region Cookie

    public void Cookie()
    {
        OnCookieUsed?.Invoke();
        foreach (var hero in InventoryManager.Instance.HeroTeam)
        {
            attackBoostTemp = (hero.AttackDamage * 30) / 100;
            damageTemp.Add(attackBoostTemp);
            hero.AttackDamage += attackBoostTemp;
            Debug.Log("Cookie Enable");
        }
        StartCoroutine(ActiveCookie30s());
    }

    private IEnumerator ActiveCookie30s()
    {
        yield return new WaitForSeconds(CookieTime);
        for (var i = 0; i < InventoryManager.Instance.HeroTeam.Count; i++)
        {
            InventoryManager.Instance.HeroTeam[i].AttackDamage -= damageTemp[i];
        }
        damageTemp.Clear();
        Debug.Log("Cookie Disable");
    }
    #endregion
    
    #region Cookie

    public void Candy()
    {
        OnCandyUsed?.Invoke();
        foreach (var hero in InventoryManager.Instance.HeroTeam)
        {
            attackBoostTemp = (hero.AttackSpeed * 30) / 100;
            speedTemp.Add(attackBoostTemp);
            hero.AttackSpeed += attackBoostTemp;
        }
        Debug.Log("Candy Enable");

        StartCoroutine(ActiveCandy30s());
    }
    
    private IEnumerator ActiveCandy30s()
    {
        yield return new WaitForSeconds(CandyTime);
        for (var i = 0; i < InventoryManager.Instance.HeroTeam.Count; i++)
        {
            InventoryManager.Instance.HeroTeam[i].AttackSpeed -= speedTemp[i];
        }
        speedTemp.Clear();
        Debug.Log("Candy Disable");
    }
    
    #endregion

    
    
    
}
