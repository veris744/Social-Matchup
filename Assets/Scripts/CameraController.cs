using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;
    public GameObject camera;
    public Vector3 offset;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void LateUpdate()
    {
        //camera.transform.position = target.position + offset;
    }
}
