using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Variables")]
    private float speed = 5f;
    private float rotateSpeed = 500f;
    private Vector3 moveinput;
    
    
    [Header("Component")]
    private Rigidbody rb;
    private Camera cam;

    [Header("Input")]
    private InputAction move;
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        move = InputSystem.actions.FindAction("Move");
    }
    
    
    void Update()
    {
        moveinput = new Vector3(move.ReadValue<Vector2>().x, 0f, move.ReadValue<Vector2>().y);
        
        Rotation();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveinput.x * speed, rb.linearVelocity.y, moveinput.z * speed);
    }


    void Rotation()
    {
        if(moveinput.sqrMagnitude == 0)return;
        
        float camY = cam.transform.eulerAngles.y;
        
        Vector3 direction = Quaternion.Euler(0, camY, 0) * moveinput;
        
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        
    }
}
