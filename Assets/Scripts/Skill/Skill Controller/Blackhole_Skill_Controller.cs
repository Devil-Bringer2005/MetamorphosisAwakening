using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    private float maxSize;
    private float growSpeed;
    private bool canGrow = true;

    private bool canAttack;
    private int attackAmount;
    private float attackCooldown;
    private float attackTimer;
    private float blackholeTimer;

    private bool canCreateHotKeys = true;

    private bool canShrink;
    private float shrinkSpeed;

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> hotKeysCreated = new List<GameObject>();

    public bool canExit;
    private bool canDisappear = true;

    public void SetUpBlackHole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _attackAmount, float _attackCooldown, float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        attackAmount = _attackAmount;
        attackCooldown = _attackCooldown;
        blackholeTimer = _blackholeDuration;

        if (SkillManager.instance.clone.crystalInsteadOfClone)
            canDisappear = false;
    }

    private void Update()
    {
        attackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if(blackholeTimer < 0 )
        {
            blackholeTimer = Mathf.Infinity;

            if(targets.Count > 0 )
            {
                ReleaseCloneAttack();
            }
            else
            {
                FinishBlackholeAbility();
            }
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        if (canAttack && attackTimer < 0 && targets.Count > 0)
        {
            attackTimer = attackCooldown;
            int attackIndex = Random.Range(0, targets.Count);

            Vector2 offset = Vector2.zero;
            if (Random.Range(0, 100) > 50)
                offset.x = 1.5f;
            else offset.x = -1.5f;

            if (SkillManager.instance.clone.crystalInsteadOfClone)
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            }
            else
            {
                SkillManager.instance.clone.CreateClone(targets[attackIndex], offset);
            }
            attackAmount--;
            
            if(attackAmount <= 0)
            {
                Invoke(nameof(FinishBlackholeAbility), 1f);
            }
        }

        if(canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        if(canShrink)
        {
            transform.localScale =Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }

    private void ReleaseCloneAttack()
    {
        if(targets.Count <= 0) return;  
        DestroyHotKeys();
        canAttack = true;
        canCreateHotKeys = false;
        if (canDisappear)
        {
            canDisappear = false;
            PlayerManager.instance.player.MakeTransparent(true);
        }
    }

    private void FinishBlackholeAbility()
    {
        DestroyHotKeys();
        canExit = true;
        canAttack = false;
        canShrink = true;
        canGrow = false;
    }

    private void DestroyHotKeys()
    {
        if(hotKeysCreated.Count > 0)
        {
            for(int i = 0; i< hotKeysCreated.Count; i++)
            {
                Destroy(hotKeysCreated[i]);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(keyCodeList.Count <= 0)
        {
            return;
        }
        if(collision.TryGetComponent(out Enemy enemy))
        {
            enemy.FreezeEnemy(true);
            CreateKey(enemy);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Enemy enemy))
        {
            enemy.FreezeEnemy(false);
        }
    }

    private void CreateKey(Enemy enemy)
    {
        if(!canCreateHotKeys) { return; }

        if(keyCodeList.Count <= 0) { return; }

        GameObject newHotKey = Instantiate(hotKeyPrefab, new Vector3(enemy.transform.position.x + 0.5f, enemy.transform.position.y + 1), Quaternion.identity);
        hotKeysCreated.Add(newHotKey);
        KeyCode newCode = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(newCode);

        Blackhole_Hotkey_Controller newController = newHotKey.GetComponent<Blackhole_Hotkey_Controller>();
        newController.SetUpHotKey(newCode, enemy.transform, this);
    }

    public void AddEnemyToList(Transform enemy) => targets.Add(enemy);
}
