using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponUpfrontNotification : MonoBehaviour
{
    public WeaponObjectManager weaponObjectManager;
    public PlayerAttack playerAttack;
    public int popUpShotCount;
    public float popUpForce;
    Transform lastEnemyHit;
    public List<Transform> markedEnemies;

    void Awake()
    {
        weaponObjectManager = FindAnyObjectByType<WeaponObjectManager>();
    }

    public void OnEnable()
    {
        popUpShotCount = 0;
        playerAttack.WeaponAssignment(3, 100, 1, 50, 2, 12, true, 0.1f, 0, null, true);
    }

    public void PopUpEffect(Transform attackedEnemy, bool precisionHit)
    {
        MarkCheck(attackedEnemy, precisionHit);
        if(attackedEnemy.GetComponentInParent<EnemyHealth>() != null)
        {
            if(popUpShotCount >= 1)
            {
                if(attackedEnemy == lastEnemyHit)
                {
                    Rigidbody rbEnemy = attackedEnemy.GetComponent<Rigidbody>();
                    if(rbEnemy != null && attackedEnemy.GetComponent<EnemyMovement>().isGrounded)
                    {
                        rbEnemy.velocity = new Vector3(rbEnemy.velocity.x, 0, rbEnemy.velocity.z);
                        rbEnemy.AddForce(attackedEnemy.up * popUpForce * rbEnemy.mass, ForceMode.Impulse);
                        markedEnemies.Add(attackedEnemy);
                        StartCoroutine(UnMarkEnemies(attackedEnemy));
                        popUpShotCount = 0;
                    }
                }
                else
                {
                    popUpShotCount = 1;
                }
            }
            else
            {
                if(attackedEnemy != lastEnemyHit)
                {
                    popUpShotCount = 1;
                }
                popUpShotCount++;
            }
            lastEnemyHit = attackedEnemy;
        }
    }

    void MarkCheck(Transform attackedEnemy, bool precisionHit)
    {
        float damageMult;
        if(markedEnemies.Contains(attackedEnemy) && attackedEnemy)
        {
            if(precisionHit)
            {
                damageMult = 10;
            }
            else
            {
                damageMult = 8;
            }
            //if()
            //attackedEnemy.GetComponent<EnemyHealth>().TakeDamage(800, null, );
        }
        else
        {
            if(precisionHit)
            {
                damageMult = 1.5f;
            }
            else
            {
                damageMult = 1;
            }
        }
        playerAttack.DealDamage(attackedEnemy.GetComponent<EnemyHealth>(), precisionHit, damageMult);
    }

    IEnumerator UnMarkEnemies(Transform markedEnemy)
    {
        yield return new WaitForSeconds(3);

        markedEnemies.Remove(markedEnemy);
    }
}
