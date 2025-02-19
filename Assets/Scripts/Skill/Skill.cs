using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    protected Player player;

    [SerializeField] protected Image cooldownUI;
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanBeUsed()
    {
        if(cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            StartCoroutine(UpdateCooldownUI());
            return true;
        }

        return false;
    }

    public virtual void UseSkill()
    {

    }

    public virtual IEnumerator UpdateCooldownUI()
    {
        if (cooldownUI == null)
            yield break;

        while (cooldownTimer > 0)
        {
            cooldownUI.fillAmount = cooldownTimer/ cooldown;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public virtual Transform NearestEnemy(Transform _targetTransform)
    {
        float closestEnemyDistance = float.MaxValue;
        Transform closestEnemy = null;

        Collider2D[] collider = Physics2D.OverlapCircleAll(_targetTransform.position, 25);

        foreach (var hit in collider)
        {
            if (hit.TryGetComponent(out Enemy enemy))
            {
                float distance = Vector2.Distance(hit.transform.position, _targetTransform.position);
                if (distance < closestEnemyDistance)
                {
                    closestEnemyDistance = distance;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }
}
