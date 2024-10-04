using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform camPos;
    bool gunReadyToFire;
    public bool removeFireCool;
    public int projectileType; //0 is hitscan, 1 is projectile
    public LayerMask gunLayer;

    //int fireRate; //Not sure if should be handled by animated
    //int readySpeed; //Not sure if should be handled by animated
    //int abilityTriggerBulletNum; //Amount of bullets used before an ability is triggered. Not sure if should make it it's seperate thing
    //int currentReloadSpeed; // Reload speed will be handled by animation
    public int selectedWeapon;
    int currentGunDamage;
    int currentBulletCount;
    float fullDamageRange;
    float damageFalloffMult;
    int magazineSize;
    bool enableDirectHits;
    float recoil;
    GameObject projToFire;
    bool specialAttack;

    public float precisionMult;



    public LineRenderer lineRenderer;
    public Animator anim;
    public PlayerMovement playerMovement;
    [Space(20)]
    int currAmmo;
    public TextMeshProUGUI ammoText;

    [Space(20)]
    public bool enableLine;



    [Space(20)]
    public WeaponPersonalSpace personalSpace;
    public WeaponTangential tangential;
    public WeaponPingPong pingPong;
    public WeaponUpfrontNotification upfrontNotification;



    float recoilTime;
    //float recoilMod;

    void Start()
    {
        gunReadyToFire = true;
    }

    public void WeaponAssignment(int newSelectedWeapon, int newGunDamage, int newBulletCount, float newFullDamageRange, float newDamageFalloffMult, int newMagazineSize, bool newEnableDirectHits, float newRecoil, int newProjectileType, GameObject newProjToFire, bool newSpecialAttack)
    {
        selectedWeapon = newSelectedWeapon;
        currentGunDamage = newGunDamage;
        //currentReloadSpeed  = newReloadSpeed;
        currentBulletCount = newBulletCount;
        fullDamageRange = newFullDamageRange;
        damageFalloffMult = newDamageFalloffMult;
        magazineSize = newMagazineSize;
        enableDirectHits = newEnableDirectHits;
        recoil = newRecoil;
        projectileType = newProjectileType;
        projToFire = newProjToFire;
        specialAttack = newSpecialAttack;


        currAmmo = magazineSize;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && gunReadyToFire && currAmmo > 0)
        {
            Fire();
            FireAnimationPlay();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            ReloadAnimation();
        }

        ammoText.text = currAmmo.ToString();
        Cheats();
        //Recoiling();
    }

    public void GunReady() //CALLED IN ANIMATION, Auto weapons can go on loop
    {
        gunReadyToFire = true;
    }

    void Fire()
    {
        if(projectileType == 0)
        {
            RaycastHit hit;
            if(Physics.Raycast(camPos.position, camPos.forward, out hit, 100, gunLayer))
            {
                if(enableLine)
                {
                    lineRenderer.SetPosition(0, camPos.position + (-transform.up * 0.25f));
                    lineRenderer.SetPosition(1, hit.point);
                    //StopCoroutine(EnableLineRender(0.2f));
                    //StartCoroutine(EnableLineRender(0.2f));
                }


                EnemyHealth enemyHealth = hit.collider.GetComponentInParent<EnemyHealth>();
                if(enemyHealth != null && enableDirectHits)
                {
                    if(hit.collider.tag == "Precision Hitbox")
                    {
                        if(!specialAttack) //Don't do damage give it to weapon only
                        {
                            DealDamage(enemyHealth, true, precisionMult);
                        }
                        HitEffect(hit, true);
                        //float finalDam = currentGunDamage * precisionMult;
                        //enemyHealth.TakeDamage((int)finalDam, this, true);
                    }
                    else
                    {
                        if(!specialAttack) //Don't do damage give it to weapon only
                        {
                            DealDamage(enemyHealth, false, 1);
                        }
                        HitEffect(hit, false);
                        //enemyHealth.TakeDamage(currentGunDamage, this, false);
                    }
                }

                if (hit.transform.gameObject.layer != 8) //If layer is not Enemy
                {
                    TerrainHitEffect(hit);
                }

                //if you hit either enemy or terrain, so anything basically
                //Use hit.layer to differentiate between terrain or enemies
            }
        }



        if(projectileType == 1)
        {
            GameObject instProj = Instantiate(projToFire, camPos.position + camPos.forward, camPos.rotation);
            instProj.GetComponent<ProjectileInfo>().playerAttack = this;
            //instProj.transform.rotation = Quaternion.LookRotation(transform.up);
        }
        currAmmo--;

        gunReadyToFire = false;
    }

    public void DealDamage(EnemyHealth enemyHealth, bool precisionHit, float damageMult)
    {
        float finalDam = currentGunDamage * damageMult;
        enemyHealth.TakeDamage((int)finalDam, this, precisionHit);
    }

    public void ReloadAll() //Called by ANIMATION
    {
        currAmmo = magazineSize;
        gunReadyToFire = true;
    }





    void ReloadAnimation()
    {
        switch(selectedWeapon)
        {
            case 0:
                anim.Play("Personal Space Reload");        
                break;
            case 1:
                anim.Play("Tangential Reload");        
                break;
            case 2:
                anim.Play("Ping Pong Reload");
                break;
            case 3:
                anim.Play("Upfront Notification Reload");
                break;
        }
    }

    void FireAnimationPlay()
    {
        switch(selectedWeapon)
        {
            case 0:
                anim.Play("Personal Space Fire");
                break;
            case 1:
                anim.Play("Tangential Fire");
                break;
            case 2:
                anim.Play("Ping Pong Fire");
                break;
            case 3:
                anim.Play("Upfront Notification Fire");
                break;
        }
    }

    void HitEffect(RaycastHit hit, bool precisionHit) //depending on what weapon is equipped
    {
        switch(selectedWeapon)
        {
            case 0:
                personalSpace.PersonalSpaceExplosion(hit);
                break;
            case 1:
                //Tangential only spawns rocket
                //tangential.PersonalSpaceExplosion(hit);
                break;
            case 3:
                upfrontNotification.PopUpEffect(hit.transform, precisionHit);
                break;
        }
    }

    void TerrainHitEffect(RaycastHit hit) //depending on what weapon is equipped
    {
        switch(selectedWeapon)
        {
            case 0:
                personalSpace.PersonalSpaceExplosion(hit);
                break;
        }
    }

    public void OnKillEffect(Transform killedEnemy, bool precisionKill) //CALLED BY ENEMYHEALTH
    {
        switch(selectedWeapon)
        {
            case 2:
                if(precisionKill)
                {
                    pingPong.PingPongKill(killedEnemy);
                }
                break;
        }
    }









    void Recoil()
    {
        recoilTime = 0.05f;
        playerMovement.addedVertRot += recoil;
    }


    void Recoiling() 
    {
        if(recoilTime > 0)
        {
            playerMovement.addedVertRot -= 0.01f;
            recoilTime -= Time.deltaTime;
        }
        else
        {
            playerMovement.addedVertRot = 0;
        }
    }




    void Cheats()
    {
        if(removeFireCool)
        {
            gunReadyToFire = true;
        }
    }

    //private IEnumerator EnableLineRender(float time)
    //{
    //    lineRenderer.enabled = true;
    //    yield return new WaitForSeconds(time);
    //    lineRenderer.enabled = false;
    //}
}
