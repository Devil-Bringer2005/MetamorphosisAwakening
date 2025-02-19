using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ability
{
    Agility,
    Rewind, 
    Slow,
    Crystal
}

[RequireComponent(typeof(CircleCollider2D))]
public class AbilityRemover : MonoBehaviour
{
    public Ability lockAbility;

    private void Awake()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            if (lockAbility == Ability.Agility)
            {
                AbilityHandler.instance.agilityPermitted = false;
            }
            else if (lockAbility == Ability.Rewind)
            {
                AbilityHandler.instance.rewindPermitted = false;
            }
            else if (lockAbility == Ability.Slow)
            {
                AbilityHandler.instance.SlowPermitted = false;
            }
            else if (lockAbility == Ability.Crystal)
            {
                AbilityHandler.instance.crystalPermitted = false;
            }

            Destroy (gameObject);
        }
    }
}
