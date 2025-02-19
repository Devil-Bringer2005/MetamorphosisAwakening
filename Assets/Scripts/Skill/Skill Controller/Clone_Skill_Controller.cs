using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackRadius;
    private float cloneTimer;

    private Transform closestEnemy;

    private bool canDuplicate;
    private int duplicateChances;
    private int facingDir = 1;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    public void SetUpClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 offset, Transform _closestEnemy, bool _canDuplicate, int _duplicateChances)
    {
        transform.position = _newTransform.position + offset;
        closestEnemy = _closestEnemy;
        FacingDir();
        canDuplicate = _canDuplicate;
        duplicateChances = _duplicateChances;
        cloneTimer = _cloneDuration;
        if(_canAttack)
        {
            anim.SetInteger("AttackCount", Random.Range(1, 4));
        }
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - Time.deltaTime);

            if(sr.color.a < 0 )
            {
                Destroy(gameObject);
            }
        }
    }


    private void AnimationTrigger()
    {
        cloneTimer = -0.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius);

        foreach (var hit in collider)
        {
            if (hit.TryGetComponent(out Enemy_AdvancedAI enemy))
            {
                enemy.DamageEffect();
                if (canDuplicate)
                {
                    if (Random.Range(0, 100) < duplicateChances)
                    {
                        SkillManager.instance.clone.CreateClone(enemy.transform, new Vector3(0.5f, 0) * facingDir);
                    }
                }
            }
        }
    }
    private void FacingDir()
    {

        if(closestEnemy)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }

    }
}
