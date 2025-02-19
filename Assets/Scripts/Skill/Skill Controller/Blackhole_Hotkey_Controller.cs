using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackhole_Hotkey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;

    private KeyCode myKeyCode;
    private TextMeshProUGUI myText;

    private Transform myEnemy;
    private Blackhole_Skill_Controller blackhole;

    public void SetUpHotKey(KeyCode _myKey, Transform _myEnemy, Blackhole_Skill_Controller _blackhole)
    {
        myKeyCode = _myKey;
        myEnemy = _myEnemy;
        blackhole = _blackhole;

        myText = GetComponentInChildren<TextMeshProUGUI>();
        sr = GetComponent<SpriteRenderer>();
        myText.text = _myKey.ToString();
    }

    private void Update()
    {
        if(Input.GetKeyDown(myKeyCode))
        {
            blackhole.AddEnemyToList(myEnemy);

            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
