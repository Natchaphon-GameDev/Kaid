using System.Collections;
using System.Collections.Generic;
using Bullet;
using Manager;
using UnityEngine;

public class Bullet_Z23 : BaseBullet
{
    private void Start()
    {
        hero = GetHeroBluePrint(HeroName.Z23_Glorious);
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
