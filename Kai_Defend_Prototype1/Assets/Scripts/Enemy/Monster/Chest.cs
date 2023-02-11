using System.Collections;
using System.Collections.Generic;
using Enemy;
using Manager;
using UnityEngine;

public class Chest : BaseEnemy
{
    public new void Init(float enemySpeed, float enemyHp, int goldDrop, Element enemyElement)
    {
        base.Init(enemySpeed, enemyHp, goldDrop, enemyElement);
    }
        
    private void Update()
    {
        Move();
        if (Camera.main is { }) rotateCanvas.transform.LookAt(Camera.main.transform);
    }

    public new void TakeDamage(float amount)
    {
        Manager.WaveManager.Instance.CheatHpTemp += amount;
        base.TakeDamage(amount);
    }
}
