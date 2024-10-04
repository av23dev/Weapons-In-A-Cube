using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rb;
    public float rocketSpeed = 5;
    public float rocketSpeedMax = 10;
    public float explosionRadius  = 10;
    public WeaponObjectManager weaponObjectManager;
    public float baseDamage = 20;
    public float distanceDamageScale;
    PlayerAttack playerAttack;
    public ProjectileInfo projectileInfo;

    public void Awake()
    {
        weaponObjectManager = FindAnyObjectByType<WeaponObjectManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(rb.velocity.magnitude < rocketSpeedMax)
        {
            rb.AddForce(transform.forward * rocketSpeedMax);
        }
    }

    void OnTriggerEnter(Collider other) //player proj will ignore player on project settings
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hitCollider in hitColliders)
        {
            EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
            if(enemyHealth != null)
            {
                //float finalDam = baseDamage + (Mathf.Exp(Vector3.Distance(hitCollider.transform.position, transform.position)) * distanceDamageScale);
                //float finalDam = baseDamage + (((Mathf.Pow(2, Vector3.Distance(hitCollider.transform.position, transform.position))* 1f) - 1f) * distanceDamageScale);
                //float finalDam = baseDamage + (Vector3.Distance(hitCollider.transform.position, transform.position) * distanceDamageScale);
                float distToExplosion = Vector3.Distance(hitCollider.transform.position, transform.position);
                float finalDam = baseDamage + ((Mathf.Pow(2, distToExplosion)* 0.3f - 0.8f) * distanceDamageScale);
                finalDam = Mathf.Clamp(finalDam, 20, 300);
                enemyHealth.TakeDamage((int)finalDam, projectileInfo.playerAttack, false);
            }
        }

        //Visualize
        GameObject instDebugSphere = Instantiate(weaponObjectManager.debugSphere, transform.position, Quaternion.identity);
        instDebugSphere.transform.localScale *= explosionRadius * 2;
        Destroy(instDebugSphere, 0.2f);

        Destroy(gameObject);
    }
}
