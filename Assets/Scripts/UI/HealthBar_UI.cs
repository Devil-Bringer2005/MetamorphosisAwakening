using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Entity entity;
    private Slider slider;
    private CharacterStats stats;
    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
        stats = GetComponentInParent<CharacterStats>();
        rectTransform = GetComponent<RectTransform>();

        entity.onFlipped += FlipHealthUI;
        stats.onHealthChanged += UpdateHealth;
        slider.maxValue = stats.GetMaxHealth();
        slider.value = stats.GetMaxHealth();
    }

    private void UpdateHealth()
    {
        slider.value = stats.currentHealth;
    }

    private void FlipHealthUI()
    {
        rectTransform.Rotate(new Vector3(0, 180, 0));
    }
}
