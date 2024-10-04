using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPersonalSpace : MonoBehaviour
{

    public WeaponObjectManager weaponObjectManager;
    public float explosionScale;
    public float maxExplosionSize;
    float explosionRadius;
    public float damageScale;
    public PlayerAttack playerAttack;

    void Start() //CHANGE LATER MAYBE
    {
        playerAttack.WeaponAssignment(0, 0, 1, 0, 0, 12, true, 0.1f, 0, null, true);
    }


    public void Awake()
    {
        weaponObjectManager = FindAnyObjectByType<WeaponObjectManager>();
    }
    public void OnEnable()
    {
        playerAttack.WeaponAssignment(0, 0, 1, 0, 0, 12, true, 0.1f, 0, null, true);
    }

    public void PersonalSpaceExplosion(RaycastHit hit)
    {
        explosionRadius = Vector3.Distance(hit.point, transform.position) * explosionScale;
        explosionRadius = Mathf.Clamp(explosionRadius, 0, maxExplosionSize);
        Collider[] hitColliders = Physics.OverlapSphere(hit.point, explosionRadius);
        foreach (Collider hitCollider in hitColliders)
        {
            EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
            if(enemyHealth != null)
            {
                float finalDam = (explosionRadius - Vector3.Distance(hitCollider.transform.position, hit.point)) * damageScale;
                finalDam = Mathf.Clamp(finalDam, 20, Mathf.Infinity);
                enemyHealth.TakeDamage((int)finalDam, playerAttack, false);
            }
        }
        
        
        //Visualize
        GameObject instDebugSphere = Instantiate(weaponObjectManager.debugSphere, hit.point, Quaternion.identity);
        instDebugSphere.transform.localScale *= explosionRadius * 2;
        Destroy(instDebugSphere, 0.2f);
    }
}
