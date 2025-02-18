using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockStrike_Controller : MonoBehaviour
{
    private CharacterStats targetStats;
    private Animator anim;

    [SerializeField] private float speed;
    private float damage;
    private bool triggered;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        triggered = false;
    }

    public void SetUp(CharacterStats _targetStats, float _damage)
    {
        targetStats = _targetStats;
        damage = _damage;
    }

    // Update is called once per frame
    void Update()
    {
        if(triggered) { return; }

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;

        if(Vector2.Distance(transform.position, targetStats.transform.position) < 0.1f)
        {
            triggered = true;

            transform.localRotation = Quaternion.identity;
            anim.transform.localRotation = Quaternion.identity;

            transform.localScale = new Vector3(3, 3);
            anim.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f);
            anim.SetTrigger("Hit");
            Invoke(nameof(OnHit), 0.2f);
        }
    }

    private void OnHit()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(damage);
        Destroy(gameObject, 0.4f);
    }
}
