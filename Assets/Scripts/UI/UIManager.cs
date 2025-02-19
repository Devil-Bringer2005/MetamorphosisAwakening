using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject crystalUI;
    public GameObject slowUI;
    public GameObject rewindUI;
    public GameObject agilityUI;

    private void Awake()
    {
        instance = this;
    }
}
