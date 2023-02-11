using System.Collections;
using System.Collections.Generic;
using Bullet;
using Manager;
using UnityEngine;

public class Bullet_F13 : BaseBullet
{
    private void Start()
    {
        hero = GetHeroBluePrint(HeroName.F13_Colorado);
        GetStatus();
    }

    private void Update()
    {
        MoveToTarget();
    }

    private new void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();;
    }
}
