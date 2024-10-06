using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The player slime transform
    public Vector3 offset;   // The offset to keep between the camera and the player
    public float smoothSpeed = 0.125f; // How smooth the camera follows

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            smoothedPosition.z = transform.position.z; // Keep the original z axis value
            transform.position = smoothedPosition;
        }
    }
}
