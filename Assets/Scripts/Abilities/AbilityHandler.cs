using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHandler : MonoBehaviour
{
    public static AbilityHandler instance;

    [Header("Abilities")]
    public bool agilityPermitted = false;
    public bool SlowPermitted = false;
    public bool crystalPermitted = false;
    public bool rewindPermitted = false;

    [Header("Cooldown")]
    public float rewindCooldown = 5f;
    private float rewindTimer;

    [Header("Temporary")]
    public Image cooldownUI;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        rewindTimer = RewindManager.Instance.HowManySecondsToTrack;
    }

    private void Update()
    {
        rewindTimer -= Time.deltaTime;
    }

    public bool CanUseRewind()
    {
        if (rewindPermitted && rewindTimer <= 0)
        {
            rewindTimer = rewindCooldown;
            StartCoroutine(UpdateCooldownUI());
            return true;
        }

        return false;
    }

    public virtual IEnumerator UpdateCooldownUI()
    {
        if (cooldownUI == null)
            yield break;

        while (rewindTimer > 0)
        {
            cooldownUI.fillAmount = rewindTimer / rewindCooldown;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
