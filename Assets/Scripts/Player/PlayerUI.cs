using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{

    public GameObject weaponMenu;
    public PlayerAttack playerAttack;
    public PlayerMovement playerMovement;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(weaponMenu.activeInHierarchy == false)
            {
                playerMovement.enabled = false;
                playerAttack.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                weaponMenu.SetActive(true);
            }
            else
            {
                playerMovement.enabled = true;
                playerAttack.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                weaponMenu.SetActive(false);
            }
        }
    }
}
