using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major Stats")]
    public Stat strength;
    public Stat intelligence;
    public Stat agility;
    public Stat vitality;

    [Header("Offencive Stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;

    [Header("Defensive Stats")]
    public Stat maxHealth;
    public Stat armour;
    public Stat evasion;
    public Stat magicresistance;

    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;

    public float ailmentTimer = 2;
    public float igniteTimer;
    public float chilledTimer;
    public float shockTimer;

    public float igniteDamageCooldown = 0.3f;
    public float igniteDamageTimer;
    public float igniteDamage;

    public float shockStrikeDamage;

    public float currentHealth;

    [SerializeField] private GameObject shockStrikePrefab;

    private EntityFX fx;

    public System.Action onHealthChanged;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHealth = GetMaxHealth();
        critPower.SetDefaultValue(150);

        fx = GetComponent<EntityFX>();
    }

    public virtual void Update()
    {
        igniteTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockTimer -= Time.deltaTime;
        igniteDamageTimer -= Time.deltaTime;

        if(igniteTimer < 0)
            isIgnited = false;
        if(chilledTimer < 0)
            isChilled = false;
        if(shockTimer < 0)
            isShocked = false;

        if(igniteDamageTimer < 0 && isIgnited)
        {
            igniteDamageTimer = igniteDamageCooldown;
            DecreaseCurrentHealthBy(igniteDamage);
        }
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (CheckTargetEvasion(_targetStats))
            return;

        float totalDamage = damage.GetValue() + strength.GetValue();

        if(CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        totalDamage = CheckTargetArmour(_targetStats, totalDamage);
        DoMagicDamage(_targetStats);
        //_targetStats.TakeDamage(totalDamage);
    }

    public void SetUpIgniteDamage(float _damage) => igniteDamage = _damage;
    private bool CanCrit()
    {
        float totalCritChance = critChance.GetValue() + agility.GetValue();
        if (Random.Range(0, 100) <= totalCritChance)
            return true;
        return false;
    }

    private float CalculateCriticalDamage(float _damage)
    {
        float totalCritDamage = (strength.GetValue() + critPower.GetValue()) * 0.01f;
        float critDamage = _damage * totalCritDamage;

        return critDamage;
    }

    private float CheckTargetArmour(CharacterStats _target, float totalDamage)
    {
        if (isChilled)
            totalDamage -= _target.armour.GetValue() * 0.8f;
        else
            totalDamage -= _target.armour.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, float.MaxValue);
        return totalDamage;
    }

    private bool CheckTargetEvasion(CharacterStats _targetStats)
    {
        float totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
        
        if (isShocked)
             totalEvasion += 20;
        
        if (Random.Range(0, 100) <= totalEvasion) 
        {
            return true;
        }
        return false;
    }

    private void DoMagicDamage(CharacterStats _targetStats)
    {
        float _fireDamage = fireDamage.GetValue();
        float _iceDamage = iceDamage.GetValue();
        float _lightningDamage = lightningDamage.GetValue();
        float totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        totalMagicDamage = CheckMagicResistance(_targetStats, totalMagicDamage);

        _targetStats.TakeDamage(totalMagicDamage);

        if (_fireDamage <= 0 && _iceDamage <= 0 && _lightningDamage <= 0)
            return;

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        while(!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value > .3 && _fireDamage > 0)
            {
                canApplyIgnite = true;
                break;
            }
            if(Random.value > .35 && _iceDamage > 0)
            {
                canApplyChill = true;
                break;
            }
            if(Random.value > .5 && _lightningDamage > 0)
            {
                canApplyShock = true;
                break;
            }
        }

        if (canApplyIgnite)
        {
            _targetStats.SetUpIgniteDamage(fireDamage.GetValue() * 0.2f);
        }

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    private float CheckMagicResistance(CharacterStats _targetStats, float totalMagicDamage)
    {
        totalMagicDamage -= _targetStats.magicresistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, float.MaxValue);
        return totalMagicDamage;
    }

    private void ApplyAilments(bool _isIgnited, bool _isChilled, bool _isShocked)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isChilled && !isIgnited && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_isIgnited && canApplyIgnite)
        {
            isIgnited = _isIgnited;
            igniteTimer = ailmentTimer;

            fx.IgniteFXFor(ailmentTimer);
        }
        if(_isChilled && canApplyChill)
        {
            isChilled = _isChilled;
            chilledTimer = ailmentTimer;

            float slowPercentage = 0.2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentTimer);
            fx.ChillFXFor(ailmentTimer);
        }
        if (_isShocked && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_isShocked);
            }
            else
            {
                if (GetComponent<Player>() != null)
                    return;

                HitNearestEnemyWithShockStrike();
            }
        }

    }

    public void ApplyShock(bool _isShocked)
    {
        if (isShocked)
            return;

        isShocked = _isShocked;
        shockTimer = ailmentTimer;

        fx.ShockFXFor(ailmentTimer);
    }

    private void HitNearestEnemyWithShockStrike()
    {
        float closestEnemyDistance = float.MaxValue;
        Transform closestEnemy = null;

        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 25);

        foreach (var hit in collider)
        {
            if (hit.TryGetComponent(out Enemy_AdvancedAI enemy))
            {
                float distance = Vector2.Distance(hit.transform.position, transform.position);
                if (distance < closestEnemyDistance && distance > 0.1)
                {
                    closestEnemyDistance = distance;
                    closestEnemy = hit.transform;
                }
            }
        }

        GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
        newShockStrike.GetComponent<ShockStrike_Controller>().SetUp(closestEnemy.GetComponent<CharacterStats>(), shockStrikeDamage);
    }

    public virtual void TakeDamage(float _damage)
    {
        _damage = 100;
        DecreaseCurrentHealthBy(_damage);

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void DecreaseCurrentHealthBy(float _damage)
    {
        currentHealth -= _damage;
        onHealthChanged?.Invoke();
    }

    public float GetMaxHealth() => maxHealth.GetValue() + vitality.GetValue() * 5;

    protected virtual void Die()
    {
        
    }
}
