using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;

        //float scale = (Camera.main.transform.position - transform.position).magnitude;
        //transform.localScale = new Vector3(scale, scale, scale) / 3;
    }
}
