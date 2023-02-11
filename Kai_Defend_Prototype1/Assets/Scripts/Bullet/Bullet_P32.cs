using System;
using System.Collections;
using System.Collections.Generic;
using Bullet;
using Hero;
using Manager;
using UnityEngine;

public class Bullet_P32 : BaseBullet
{
    private void Start()
    {
        hero = GetHeroBluePrint(HeroName.P32_Enterprise);
        GetStatus();
    }

    private void Update()
    {
        MoveToTarget();
    }

    private new void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
    }
}