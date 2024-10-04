using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumFloat : MonoBehaviour
{

    void Start()
    {
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
        
        float scale = (Camera.main.transform.position - transform.position).magnitude;
        transform.localScale = new Vector3(scale, scale, scale) / 10f;

        transform.position += transform.up * 0.005f;
    }
}
