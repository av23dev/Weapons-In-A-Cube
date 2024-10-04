using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{  
    public int maxHealth = 1000;
    public int health;
    public GameObject damageNum;
    public Slider slider;
    public bool enableRespawn = true;
    public GameObject newDummy; //If I intantiate this obj, then it makes it same values, not sure what problems it can cause
    GeneralObjectManager generalObjectManager;
    PlayerAttack storedAttackerPlayerAttack;
    public bool trueDeath;

    void Awake()
    {
        health = maxHealth;
        generalObjectManager = FindAnyObjectByType<GeneralObjectManager>();
    }

    public void Update()
    {
        slider.value = (float)health / maxHealth;
    }

    public void TakeDamage(int damage, PlayerAttack attackerPlayerAttackScript, bool precisionHit)
    {
        GameObject instDamNum = Instantiate(damageNum, transform.position + transform.up, Quaternion.identity); //num destroy is handled by the obj itself
        instDamNum.GetComponentInChildren<TextMeshProUGUI>().text = damage.ToString();

        if(precisionHit)
        {
            instDamNum.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
        }

        health -= damage;
        
        if(attackerPlayerAttackScript != null)
        {
            storedAttackerPlayerAttack = attackerPlayerAttackScript;
        }

        if(health <= 0)
        {
            Die(precisionHit);
        }
    }

    void Die(bool precisionHit)
    {
        gameObject.SetActive(false);

        if(enableRespawn)
        {
            Invoke("Respawn", 2);
        }
        
        if(trueDeath)
        {
            Destroy(gameObject, 2);
        }

        if(storedAttackerPlayerAttack != null)
        {
            storedAttackerPlayerAttack.OnKillEffect(transform, precisionHit);
        }
        //if(enableRespawn)
        //{
        //    Instantiate(generalObjectManager.testDummy, transform.position + transform.up, Quaternion.identity);
        //}
        //Destroy(gameObject);
    }

    void Respawn()
    {
        health = maxHealth;
        gameObject.SetActive(true);
    }
}
