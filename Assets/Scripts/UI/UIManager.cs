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

    public Image livesUI;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateLivesUI(int amount)
    {
        if (amount == 1)
        {
            livesUI.fillAmount = 0.2f;
        }
        else if (amount == 2)
        {
            livesUI.fillAmount = 0.4f;
        }
        else if (amount == 3)
        {
            livesUI.fillAmount = 0.6f;
        }
    }
}
