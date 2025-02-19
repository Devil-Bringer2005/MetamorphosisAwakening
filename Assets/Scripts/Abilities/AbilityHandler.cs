using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHandler : MonoBehaviour
{
    public static AbilityHandler instance;

    [Header("Abilities")]
    public bool agilityPermitted = false;
    public bool SlowPermitted = false;
    public bool crystalPermitted = false;
    public bool rewindPermitted = false;

    private void Awake()
    {
        instance = this;
    }
}
