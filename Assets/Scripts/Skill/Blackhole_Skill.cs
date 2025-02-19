using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill : Skill
{
    [SerializeField] private GameObject blackholePrefab;

    [SerializeField] private float maxSize = 12f;
    [SerializeField] private float growSpeed = 15f;
    [SerializeField] private float shrinkSpeed = 2f;
    [SerializeField] private float blackholeDuration = 3f;
    [Space]
    [SerializeField] private int attackAmount = 7;
    [SerializeField] private float attackCooldown = 0.7f;

    Blackhole_Skill_Controller controller;

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

        GameObject newBlackHole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);
        controller  = newBlackHole.GetComponent<Blackhole_Skill_Controller>();

        controller.SetUpBlackHole(maxSize, growSpeed, shrinkSpeed, attackAmount, attackCooldown, blackholeDuration);
    }
    public override bool CanBeUsed()
    {
        return base.CanBeUsed();
    }

    public bool SKillCompleted()
    {
        if(controller == null) { return false; }

        if(controller.canExit)
        {
            controller = null;
            return true;
        }

        return false;
    }

    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }
}
