using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPingPong : MonoBehaviour
{
    public WeaponObjectManager weaponObjectManager;
    public PlayerAttack playerAttack;
    public float explosionRadius = 5;
    public int pingPongCount;
    public int pingPongCountMax;
    float pingPongCooldown;
    public float pingPongCooldownMax;
    public Collider[] hitColliders;
    bool pingPongCountOnce;
    public int explosionDam;
    public List<Transform> affectedEnemies; //affected enemies are the ones generating explosion
    public List<Transform> tempAffectedEnemies;//tempAffected are the ones being hit with the explosion. They generate explosion on next cycle
    //int affectedEnemiesCount;

    public void Awake()
    {
        weaponObjectManager = FindAnyObjectByType<WeaponObjectManager>();
    }

    void Update()
    {
        if(pingPongCooldown <= 0)
        {
            pingPongCount = 0;
        }
        else
        {
            pingPongCooldown -= Time.deltaTime;
        }
    }

    public void OnEnable()
    {
        playerAttack.WeaponAssignment(2, 100, 1, 50, 2, 12, true, 0.1f, 0, null, false);
    }

    public void PingPongKill(Transform killedEnemy)
    {
        tempAffectedEnemies.Clear();
        if(pingPongCooldown <= 0)
        {
            if(killedEnemy != null)
            {
                affectedEnemies.Add(killedEnemy); //Add the killed enemy to the list on first kill
            }
            pingPongCooldown = pingPongCooldownMax;
        }

        if(pingPongCount < pingPongCountMax) //Only 3 times
        {
/////////////////////
            for(int i = 0; i < affectedEnemies.Count; i++)
            {
                if(affectedEnemies[i].gameObject.activeInHierarchy || affectedEnemies[i].transform == killedEnemy)
                {
                    hitColliders = Physics.OverlapSphere(affectedEnemies[i].position, explosionRadius); //DETECT ALL COLLIDERS IN AREA
                    for(int j = 0; j < hitColliders.Length; j++)
                    {
                        if(hitColliders[j] != null && hitColliders[j].gameObject.activeInHierarchy == true) //Check if list is null, don't hit self, not dead
                        {
                            EnemyHealth enemyHealth = hitColliders[j].GetComponent<EnemyHealth>(); //Find which objects are enemies w/ script
                            if(enemyHealth != null)
                            {
                                if(!affectedEnemies.Contains(hitColliders[j].transform) && !tempAffectedEnemies.Contains(hitColliders[j].transform) && hitColliders[j].transform != affectedEnemies[i]) //DOn't affect twice
                                {
                                    tempAffectedEnemies.Add(hitColliders[j].transform); //ISOLATE ENEMIES FROM ALL COLLIDERS
                                    enemyHealth.TakeDamage(explosionDam, null, false);
                                }
                            }
                        }   
                    }
                    //Visualize
                    GameObject instDebugSphere = Instantiate(weaponObjectManager.debugSphere, affectedEnemies[i].position, Quaternion.identity);
                    instDebugSphere.transform.localScale *= explosionRadius * 2;
                    Destroy(instDebugSphere, 0.15f);
                }
            }

            affectedEnemies.Clear();
            affectedEnemies.AddRange(tempAffectedEnemies);
            StartCoroutine(PingPongExplosion()); //Loop it after a second

            //foreach(Transform enemiesHit in affectedEnemies)
            //{
            //    PingPongExplosion(affectedEnemies);
            //    enemyHealth.TakeDamage(explosionDam, playerAttack, false);
            //}
            

            //if(pingPongCountOnce)
            //{
            //    pingPongCount++;
            //    pingPongCountOnce = false;
            //}

        }
        
        if(pingPongCount >= 3)
        {
            affectedEnemies.Clear();
        }
    }

    IEnumerator PingPongExplosion()
    {
        yield return new WaitForSeconds(1);
        pingPongCount++;
        PingPongKill(null);


        ////Visualize
        //GameObject instDebugSphere = Instantiate(weaponObjectManager.debugSphere, affectedEnemy.position, Quaternion.identity);
        //instDebugSphere.transform.localScale *= explosionRadius * 2;
        //Destroy(instDebugSphere, 0.2f);
    }
}
