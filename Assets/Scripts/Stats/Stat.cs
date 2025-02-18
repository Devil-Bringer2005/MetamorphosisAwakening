using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private float baseValue;

    public List<float> modifiers = new List<float>();

    public void SetDefaultValue(float _value)
    {
        baseValue = _value;
    }

    public float GetValue()
    {
        float finalVlalue  = baseValue;

        foreach (var modifier in modifiers)
            finalVlalue += modifier;

        return finalVlalue;
    }

    public void AddModifier(float modifier)
    {
        modifiers.Add(modifier);
    }

    public void RemoveModifier(float modifier)
    {
        modifiers.Remove(modifier);
    }
}
