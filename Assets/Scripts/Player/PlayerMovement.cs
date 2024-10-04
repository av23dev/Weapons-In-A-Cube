using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1f;      // Movement speed
    public float maxMoveSpeed = 5f;
    public float maxFallSpeed = 10;
    public float jumpForce = 5f;      // Jumping force
    public float gravityForce = 2f; // Custom gravity multiplier
    public float deccelerationRate = 0.70f;

    public float groundedOffset;
    public float mouseSens;


    public Transform playerObj;
    public Transform camObj;
    public Rigidbody rb;


    private bool isGrounded;
    public float xRotation = 0f;

    float mouseX;
    float mouseY;
    float storedMoveSpeed;
    public LayerMask groundLayerMask;

    public float mouseSpeedVariable;
    public float addedVertRot;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        storedMoveSpeed = moveSpeed;
    }

    void Update()
    {
        Jump();
        GroundCheck();
    }

    void LateUpdate()
    {
        LookAround();
    }

    void FixedUpdate()
    {
        Move();
        Gravity();
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

    void Move()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");


        Vector3 movementDir = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;
        
        if (rb.velocity.magnitude < maxMoveSpeed)
        {
            rb.AddRelativeForce(movementDir * moveSpeed);
        }

        Vector3 onlyXZVector = rb.velocity;
        onlyXZVector.x = onlyXZVector.x * deccelerationRate;
        onlyXZVector.z = onlyXZVector.z * deccelerationRate;
        rb.velocity = onlyXZVector;
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        xRotation -= mouseY;
        //xRotation = -mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camObj.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerObj.Rotate(Vector3.up * mouseX);
    }

    void Jump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void GroundCheck()
    {
        Vector3 lowerPlayerTransform = transform.position;
        lowerPlayerTransform.y = lowerPlayerTransform.y - groundedOffset;

        isGrounded = Physics.CheckSphere(lowerPlayerTransform, 0.45f, groundLayerMask);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 lowerPlayerTransform = transform.position;
        lowerPlayerTransform.y = lowerPlayerTransform.y - groundedOffset;

        Gizmos.DrawSphere(lowerPlayerTransform, 0.45f);
    }
}
