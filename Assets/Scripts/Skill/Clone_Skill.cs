using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]
    [SerializeField] private bool canAttack;
    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;
    [SerializeField] private bool createCloneOnCounterAttack;

    [Header("Clone Duplication")]
    [SerializeField] private bool canCloneDuplicate;
    [SerializeField][Range(0,100)]private int duplicateChances;

    [Header("Crystal Inplace Of Clone")]
    public bool crystalInsteadOfClone;

    public void CreateClone(Transform newTransform, Vector3 offest)
    {
        if(crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        GameObject clone = Instantiate(clonePrefab);
        clone.GetComponent<Clone_Skill_Controller>().SetUpClone(newTransform, cloneDuration, canAttack, offest, NearestEnemy(clone.transform), canCloneDuplicate, duplicateChances);
    }

    public void CreateCloneOnDashStart()
    {
        if (createCloneOnDashStart)
            CreateClone(player.transform, Vector3.zero);
    }

    public void CreateCloneOnDashOver()
    {
        if (createCloneOnDashOver)
            CreateClone(player.transform, Vector3.zero);
    }

    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if (createCloneOnCounterAttack)
            StartCoroutine(CraeteCloneWithDelay(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
    }

    private IEnumerator CraeteCloneWithDelay(Transform transform, Vector3 offset)
    {
        yield return new WaitForSeconds(0.2f);
        CreateClone(transform, offset);
    }
}
