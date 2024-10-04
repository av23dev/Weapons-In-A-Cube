using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public bool isGrounded;
    public Rigidbody rb;
    public float maxFallSpeed = 10;
    public float gravityForce = 4f;
    public float groundedOffset = 0.75f;
    public LayerMask groundLayerMask;
    public float deccelerationRate = 0.90f;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        GroundCheck();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Gravity();
        Move();
    }

    void Gravity()
    {
        // Apply custom gravity
        if (!isGrounded)
        {
            if (rb.velocity.y > -maxFallSpeed)
            {
                rb.AddForce(transform.up * -gravityForce * rb.mass);
            }
        }
    }

    void GroundCheck()
    {
        Vector3 lowerPlayerTransform = transform.position;
        lowerPlayerTransform.y = lowerPlayerTransform.y - groundedOffset;

        isGrounded = Physics.CheckSphere(lowerPlayerTransform, 0.45f, groundLayerMask);
    }

    void Move()
    {
        Vector3 onlyXZVector = rb.velocity;
        onlyXZVector.x = onlyXZVector.x * deccelerationRate;
        onlyXZVector.z = onlyXZVector.z * deccelerationRate;
        rb.velocity = onlyXZVector;
    }
}
