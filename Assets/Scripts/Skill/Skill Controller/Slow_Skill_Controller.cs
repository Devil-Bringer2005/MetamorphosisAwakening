using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class Slow_Skill_Controller : MonoBehaviour
{
    private float maxSize;
    private float growSpeed;
    private bool canGrow = true;

    private float slowTimer;
    private float slowStartTime = 0;

    private bool canShrink;
    private float shrinkSpeed;
    private float multiplier;

    public void SetUpSlow(float _maxSize, float _growSpeed, float _shrinkSpeed, float _slowDuration, float _multiplier)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        
        slowTimer = _slowDuration;
        multiplier = _multiplier;
        slowStartTime = Time.time;
    }


    private void Update()
    {
        transform.position = PlayerManager.instance.player.transform.position;

        if (Time.time > slowStartTime + slowTimer)
        {
            canGrow = false;
            canShrink = true;
        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO: Implement for projectiles

        if (collision.TryGetComponent(out ISlowable slowableEntity))
        {
            slowableEntity.Slow(true, multiplier);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //TODO: Implement for projectiles

        if (collision.TryGetComponent(out ISlowable slowableEntity))
        {
            slowableEntity.Slow(false);
        }
    }

}
