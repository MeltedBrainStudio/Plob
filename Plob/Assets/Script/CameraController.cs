using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Setting")]
    public float CameraSpeed;

    private void Update()
    {
        Vector3 _newPosition = transform.position + Vector3.right * CameraSpeed * Time.deltaTime;
        transform.position = _newPosition;
    }
}
