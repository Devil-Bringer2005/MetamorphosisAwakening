using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow_Skill : Skill
{
    [SerializeField] private GameObject slowAreaPrefab;

    [SerializeField] private float maxSize = 12f;
    [SerializeField] private float growSpeed = 15f;
    [SerializeField] private float shrinkSpeed = 2f;
    [SerializeField] private float slowDuration = 3f;
    [SerializeField]
    [Range(0, 1)] private float multiplier = 0.3f;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackHole = Instantiate(slowAreaPrefab, player.transform.position, Quaternion.identity);
        newBlackHole.GetComponent<Slow_Skill_Controller>().SetUpSlow(maxSize, growSpeed, shrinkSpeed, slowDuration, multiplier);
    }

    public override bool CanBeUsed()
    {
        return base.CanBeUsed();
    }

    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }
}
