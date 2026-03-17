using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Variables")]
    private float speed = 5.5f;
    
    //Dash
    private float dashSpeed = 35f;
    private bool isdashing = false;
    private float dashCooldown = 1.5f;
    private bool Candash = true;
    
    //Jump
    private bool isGrounded;
    private float JumpForce = 6f;
    private float fallMultipler = 1.5f;
    
    
    
    private float rotateSpeed = 5f;
    
    
    
    private Vector3 moveinput;
    private Vector3 direction;
    
    
    [Header("Component")]
    private Rigidbody rb;
    private GameObject camPivot;

    [Header("Input")]
    private PlayerInput playerInput;
    private InputAction move;
    private InputAction dash;
    private InputAction jump;
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        camPivot = GameObject.Find("CameraPivot");
        
        move = playerInput.actions["Move"];
        dash = playerInput.actions["Dash"];
        jump = playerInput.actions["Jump"];
    }


    void Update()
    {
        Vector2 inputEnter = move.ReadValue<Vector2>();
        if (inputEnter.magnitude < 0.15f){
            inputEnter = Vector3.zero;
        }
        moveinput = new Vector3(inputEnter.x, 0f, inputEnter.y);
        
        DirectionCalcul();
       // Rotation();
        
        if (dash.WasPressedThisFrame() && Candash && isGrounded)
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

        if (rb.linearVelocity.y < 0)
        {
            rb.AddForce(Vector3.up * Physics.gravity.y * fallMultipler, ForceMode.Acceleration); //Fonctionne -> rend une chute plus rapide avec un Multiplicateur
        }
    }


    void Rotation()
    {
        if (direction.sqrMagnitude > 0.001f)
        {
            Vector3 flatDir = new Vector3(direction.x, 0f, direction.z);
            flatDir = flatDir.normalized;
            
            Quaternion targetRotation = Quaternion.LookRotation(flatDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
        } 
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
        Vector3 forward = camPivot.transform.forward;
        Vector3 right = camPivot.transform.right;
        
        right.y = 0;
        forward.y = 0;
            
        forward.Normalize();
        right.Normalize();
            
        direction = forward * moveinput.z + right * moveinput.x;
        direction.Normalize();
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
