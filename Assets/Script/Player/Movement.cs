using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Variables")]
    public float speed = 5f;
    private Vector3 moveinput;
    
    
    [Header("Component")]
    private Rigidbody rb;

    [Header("Input")]
    private InputAction move;
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        move = InputSystem.actions.FindAction("Move");
    }
    
    
    void Update()
    {
        moveinput = new Vector3(move.ReadValue<Vector2>().x, 0f, move.ReadValue<Vector2>().y);

    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveinput.x * speed, rb.linearVelocity.y, moveinput.z * speed);
    }
}
