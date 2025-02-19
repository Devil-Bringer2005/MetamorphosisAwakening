using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius);
        
        foreach(var hit in  collider)
        {
            if(hit.TryGetComponent(out Enemy_AdvancedAI enemy))
            {
                player.Stats.DoDamage(enemy.GetComponent<EnemyStats>());
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
