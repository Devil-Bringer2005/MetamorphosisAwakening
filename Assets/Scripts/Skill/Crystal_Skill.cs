using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    [Space]
    [SerializeField] private float crystalDuration;

    [Header("Mirage info")]
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Explosion info")]
    [SerializeField] private bool canExplode;

    [Header("Move info")]
    [SerializeField] private bool canMove;
    [SerializeField] private float moveSpeed;

    [Header("Multicrystal info")]
    [SerializeField] private bool canUseMulti;
    [SerializeField] private int crystalAmount;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float resetWindow;
    private List<GameObject> crystalLeft = new List<GameObject>();

    private GameObject currentCrystal;
    public override void UseSkill()
    {
        base.UseSkill();

        if(CanUseMultiCrystal()) { return; }

        if (currentCrystal == null)
            CreateCrystal();
        else
        {
            if (canMove) { return; }
            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if (cloneInsteadOfCrystal)
            {
                player.skill.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
            }
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentController = currentCrystal.GetComponent<Crystal_Skill_Controller>();

        currentController.SetUpCrystal(crystalDuration, canExplode, canMove, moveSpeed, NearestEnemy(currentCrystal.transform));
    }

    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();

    private bool CanUseMultiCrystal()
    {
        if(canUseMulti)
        {
            if (crystalLeft.Count <= 0)
                RefilCrystal();
            if(crystalLeft.Count > 0)
            {

                if (crystalAmount == crystalLeft.Count)
                    Invoke(nameof(ResetAbility), resetWindow);

                cooldown = 0;
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);
                

                Crystal_Skill_Controller newController = newCrystal.GetComponent<Crystal_Skill_Controller>();
                newController.SetUpCrystal(crystalDuration, canExplode, canMove, moveSpeed, NearestEnemy(newCrystal.transform)); ;

                crystalLeft.Remove(crystalToSpawn);
                
                if(crystalLeft.Count == 0)
                {
                    cooldown = multiStackCooldown;
                    RefilCrystal();
                }
                return true;
            }
        }

        return false;
    }

    private void RefilCrystal()
    {
        int amountToAdd = crystalAmount - crystalLeft.Count;

        for(int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldown > 0) { return; }

        cooldown = multiStackCooldown;
        RefilCrystal();
    }
}
