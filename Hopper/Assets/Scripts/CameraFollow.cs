using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Frog;
    public float cameraDistance = 10.0f;

    private void Awake()
    {
        GetComponent<Camera>().orthographicSize = ((Screen.height / 2) / cameraDistance);
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, Frog.position.y, transform.position.z);
    }

}
