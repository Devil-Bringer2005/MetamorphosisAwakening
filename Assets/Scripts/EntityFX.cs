using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private float flashDuration;
    [SerializeField] private Material flashFX;
    private Material originalMaterial;

    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] shockColor; 

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }

    public IEnumerator FlashFX()
    {
        spriteRenderer.material = flashFX;

        Color currentColor = spriteRenderer.color;
        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.color = currentColor;

        spriteRenderer.material = originalMaterial;
    }

    public void RedWhiteBlink()
    {
        if(spriteRenderer.color != Color.white)
            spriteRenderer.color = Color.white;
        else
            spriteRenderer.color = Color.red;
    }

    public void IgniteFXFor(float _seconds)
    {
        InvokeRepeating(nameof(IgniteColorFX), 0, 0.2f);
        Invoke(nameof(CancelColorChange), _seconds);
    }

    public void ChillFXFor(float _seconds)
    {
        InvokeRepeating(nameof(ChillColorFX), 0, 0.2f);
        Invoke(nameof(CancelColorChange), _seconds);
    }

    public void ShockFXFor(float _seconds)
    {
        InvokeRepeating(nameof(ShockColorFX), 0, 0.2f);
        Invoke(nameof(CancelColorChange), _seconds);
    }

    private void IgniteColorFX()
    {
        if(spriteRenderer.color == igniteColor[0])
            spriteRenderer.color = igniteColor[1];
        else
            spriteRenderer.color = igniteColor[0];
    }

    private void ChillColorFX()
    {
        if (spriteRenderer.color == chillColor[0])
            spriteRenderer.color = chillColor[1];
        else
            spriteRenderer.color = chillColor[0];
    }

    private void ShockColorFX()
    {
        if (spriteRenderer.color == shockColor[0])
            spriteRenderer.color = shockColor[1];
        else
            spriteRenderer.color = shockColor[0];
    }

    public void CancelColorChange()
    {
        CancelInvoke();
        spriteRenderer.color = Color.white;
    }
}
