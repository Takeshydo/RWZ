using System;
using UnityEngine;
using UnityEngine.InputSystem;  

public class CameraView : MonoBehaviour
{
    [Header("Components")] 
    public Transform springArm;
    public Transform playerPos;
    public Transform camPos;
    public Transform camPivot;
    
    
    private InputAction lookActions;
    private PlayerInput playerInput;
    
    [Header("Variables")]
    private Vector2 moveCam;
    
    private float sensibility = 100f;
    private float smoothSpeed = 10f;
    private float autoFollowSpd = 5f;
    
    //Var Rotation Axe Camera
    private float pitch = 20f;
    private float yaw = 0f;
    
    //Var SphereCast
    private float sphereRadius = 0.3f;
    public LayerMask collisionMask;
    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        
        lookActions = playerInput.actions["Look"];
    }

    private void Update()
    {
        moveCam = lookActions.ReadValue<Vector2>();
        if (moveCam.magnitude < 0.15f)
        { 
            moveCam = Vector2.zero;
        }
    }

    void LateUpdate()
    {
        
        float yawC = moveCam.x * sensibility * Time.deltaTime;
        float pitchC = moveCam.y * sensibility * Time.deltaTime;
        
        pitch -= pitchC;
        pitch = Mathf.Clamp(pitch, -30f, 70f);
        yaw += yawC;
        
        camPivot.localRotation = Quaternion.Euler(pitch, yaw, 0f);
        
        Vector3 desiredPos = springArm.position;
        Vector3 direction = desiredPos - camPivot.position;
        float maxDistance = direction.magnitude;
        direction.Normalize();
        
        RaycastHit hit;


        if (Physics.SphereCast(camPivot.position, sphereRadius, direction, out hit, maxDistance, collisionMask))
        {
            camPos.position = hit.point + hit.normal * 0.2f;
        }
        else
        {
            camPos.position = Vector3.Lerp(camPos.position, desiredPos, smoothSpeed * Time.deltaTime);
        }
        
        camPos.LookAt(camPivot.position);
    }
}
