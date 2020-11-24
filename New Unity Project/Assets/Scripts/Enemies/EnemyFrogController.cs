using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFrogController : EnemyControllerBase
{
    public override void TakeDamage(int damage, DamageType type = DamageType.Casual, Transform palyer = null)
    {
        if (type != DamageType.Projectile)
            return;

        base.TakeDamage(damage, type, palyer);
    }
}