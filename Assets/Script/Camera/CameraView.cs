using UnityEngine;

public class CameraView : MonoBehaviour
{
    [Header("Components")]
    private Transform playerPos;
    void Start()
    {
        playerPos = GetComponentInParent<Transform>();
    }

    void LateUpdate()
    {
        
    }
}
