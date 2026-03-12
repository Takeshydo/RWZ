using UnityEngine;
using UnityEngine.InputSystem;  

public class CameraView : MonoBehaviour
{
    [Header("Components")]
    private Transform SpringArm; //Point d'encrage fixe
    private Transform playerPos; 
    private InputAction lookActions;
    
    [Header("Variables")]
    private Vector2 moveCam;

    private float offset = 6f;
    private float sensibility = 75f;
    private float minPitch = -25f;
    private float maxPitch = 50f;
    
    //Var Rotation Axe Camera
    private float yaw;
    private float pitch;
    
    void Start()
    {
        lookActions = InputSystem.actions.FindAction("Look");
        SpringArm = transform.parent;
    }

    void Update()
    {
        moveCam = lookActions.ReadValue<Vector2>();

        moveCam.y *= -1;
        
        //Movement sur yaw 
        yaw += moveCam.x * sensibility *  Time.deltaTime;
        //Movement sur pitch
        pitch += moveCam.y * sensibility *  Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    void LateUpdate()
    {
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        transform.position = SpringArm.position - transform.forward * offset;
    }
}
