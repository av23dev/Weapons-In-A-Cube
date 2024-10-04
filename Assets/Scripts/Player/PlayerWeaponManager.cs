using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public GameObject[] allWeapons;
    public string[] weaponNames;
    int weaponNum;
    //GameObject[] weaponToEnable;
    public TextMeshProUGUI weaponNameText;

    public void Start()
    {
        DisableAllButOneWeapons(0);
    }

    public void DisableAllButOneWeapons(int weaponNum)
    {
        foreach(GameObject weapon in allWeapons)
        {
            weapon.SetActive(false);
        }

        weaponNameText.text = weaponNames[weaponNum];
        allWeapons[weaponNum].SetActive(true);
    }
}
