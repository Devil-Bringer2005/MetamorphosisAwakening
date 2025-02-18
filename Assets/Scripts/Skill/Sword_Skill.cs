using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Bounce info")]
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpedd = 20f;

    [Header("Pierce info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("spin info")]
    [SerializeField] private float maxDistance;
    [SerializeField] private float spinDuration;
    [SerializeField] private float spinGravity;
    [SerializeField] private float hitCooldown;


    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeDuration;
    [SerializeField] private float returningSpeed = 15f;

    private Vector2 finalDir;

    [SerializeField] private int numerOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();
        GenerateDots();
        SetUpGravity();
    }

    private void SetUpGravity()
    {
        if (swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if (swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
        else if (swordType == SwordType.Spin)
            swordGravity = spinGravity;
        else if (swordType == SwordType.Regular)
            swordGravity = 4.5f;
    }

    protected override void Update()
    {
        base.Update();
        finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);

        for(int i = 0; i < dots.Length; i++)
        {
            dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
        }
    }
    public void CreateSword()
    {
        GameObject sword = Instantiate(swordPrefab, player.transform.position, player.transform.rotation);
        Sword_Skill_Controller swordController = sword.GetComponent<Sword_Skill_Controller>();

        if (swordType == SwordType.Bounce)
        {
            swordController.SetUpBounce(true, bounceAmount, bounceSpedd);
        }
        else if (swordType == SwordType.Pierce)
        {
            swordController.SetUpPierce(pierceAmount);
        }
        else if(swordType == SwordType.Spin)
        {
            swordController.SetUpSpin(true, maxDistance, spinDuration, hitCooldown);
        }
        player.AssignNewSword(sword);
        DotActive(false);
        swordController.SetUpSword(finalDir, swordGravity, player, freezeDuration, returningSpeed);
    }

    #region Aim Sword
    private Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direcion = mousePosition - playerPosition;

        return direcion;
    }

    public void DotActive(bool _isActive)
    { 
        for(int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }
    private void GenerateDots()
    {
        dots = new GameObject[numerOfDots];

        for(int i = 0; i< numerOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab,player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + 0.5f * (Physics2D.gravity * swordGravity)* (t*t) ;

        return position;
    }
    #endregion
}
