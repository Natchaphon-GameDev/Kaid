using System.Collections;
using System.Collections.Generic;
using Bullet;
using Manager;
using UnityEngine;

public class Bullet_Z1 : BaseBullet
{
    private void Start()
    {
        hero = GetHeroBluePrint(HeroName.Z1_Hornet);
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
