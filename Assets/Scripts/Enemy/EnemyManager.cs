using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] dummies;

    public void Awake()
    {
        dummies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    
}
