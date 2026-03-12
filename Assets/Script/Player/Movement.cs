using System;
using System.Collections;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Variables")]
    private float speed = 5f;
    
    //Dash
    private float dashSpeed = 25f;
    private bool isdashing = false;
    private float dashCooldown = 1.5f;
    private bool Candash = true;
    
    //Jump
    private bool isGrounded;
    private float JumpForce = 5f;
    
    
    
    private float rotateSpeed = 500f;
    
    
    
    private Vector3 moveinput;
    private Vector3 direction;
    
    
    [Header("Component")]
    private Rigidbody rb;
    private Camera cam;

    [Header("Input")]
    private InputAction move;
    private InputAction dash;
    private InputAction jump;
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        move = InputSystem.actions.FindAction("Move");
        dash = InputSystem.actions.FindAction("Dash");
        jump = InputSystem.actions.FindAction("Jump");
    }
    
    
    void Update()
    {
        moveinput = new Vector3(move.ReadValue<Vector2>().x, 0f, move.ReadValue<Vector2>().y);
        
        DirectionCalcul();
        Rotation();

        if (dash.WasPressedThisFrame() && Candash)
        {
            StartCoroutine(Slide());
        }

        if (jump.WasPressedThisFrame() && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        Move();
    }


    void Rotation()
    {
        if (moveinput.sqrMagnitude < 0.1f) return;
        
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    void Move()
    {
        if (isdashing) return;
        
        Vector3 velocity = direction * speed;
        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
    }
    
    void Jump()
    {
        rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);     
    }

    void DirectionCalcul()
    {
        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;
        
        right.y = 0;
        forward.y = 0;
        
        forward.Normalize();
        right.Normalize();
        
        direction = forward * moveinput.z + right * moveinput.x;
    }

    IEnumerator  Slide()
    {
        if (isdashing) yield break;
        
        Candash = false;
        isdashing = true;
        
        float dashDuration = 0.5f;
        float timer = 0f;
        
        Vector3 dashDir = direction.normalized;
        float currentSpeed = dashSpeed;


        while (timer < dashDuration)
        {
            rb.linearVelocity = new Vector3(dashDir.x * currentSpeed, rb.linearVelocity.y, dashDir.z * currentSpeed);
            currentSpeed = Mathf.Lerp(dashSpeed, 0, timer/dashDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        
        isdashing = false;
        
        yield return new WaitForSeconds(dashCooldown);
        Candash = true;
    }


    private void OnCollisionStay(Collision collision)
    {
            isGrounded = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
