using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOverheadFollow : MonoBehaviour
{
    public Transform followObject;
    public bool follow = true;

    public float followSpeed = 6f;
    public Vector3 offset;


    void FixedUpdate()
    {
        if (follow && followObject != null)
        {
            Vector3 targetPos = followObject.position + offset;
            Vector3 lerpedPos = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
            transform.position = lerpedPos;
        }
    }

    public void SetFollowObject(Transform newObject) {
        followObject = newObject;
    }

    public void SetFollowSpeed(float s) {
        followSpeed = s;
    }
}
