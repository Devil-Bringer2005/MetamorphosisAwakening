using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy_AdvancedAI enemy;
    protected override void Start()
    {
        base.Start();
        enemy = GetComponent<Enemy_AdvancedAI>();
    }

    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);

        enemy.DamageEffect();
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();
    }
}
