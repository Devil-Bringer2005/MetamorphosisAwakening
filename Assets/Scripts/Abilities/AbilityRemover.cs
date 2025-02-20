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
    public GameObject ActivateDialogue;

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
                UIManager.instance.agilityUI.SetActive(false);
            }
            else if (lockAbility == Ability.Rewind)
            {
                AbilityHandler.instance.rewindPermitted = false;
                UIManager.instance.rewindUI.SetActive(false);
            }
            else if (lockAbility == Ability.Slow)
            {
                AbilityHandler.instance.SlowPermitted = false;
                UIManager.instance.slowUI.SetActive(false);
            }
            else if (lockAbility == Ability.Crystal)
            {
                AbilityHandler.instance.crystalPermitted = false;
                UIManager.instance.crystalUI.SetActive(false);
            }

            ActivateDialogue.SetActive(true);
            Destroy (gameObject);
        }
    }
}
