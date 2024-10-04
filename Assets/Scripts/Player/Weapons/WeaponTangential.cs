using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTangential : MonoBehaviour
{
    
    public WeaponObjectManager weaponObjectManager;
    public float explosionScale;
    public float maxExplosionSize;
    float explosionRadius;
    public float damageScale;
    public PlayerAttack playerAttack;
    public GameObject rocket;
    public void Awake()
    {
        weaponObjectManager = FindAnyObjectByType<WeaponObjectManager>();
    }

    public void OnEnable()
    {
        playerAttack.WeaponAssignment(1, 0, 1, 0, 0, 12, false, 0.1f, 1, rocket, false);
    }
}
