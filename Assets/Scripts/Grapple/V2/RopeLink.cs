using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeLink : MonoBehaviour
{
    public float z = 0;

    private float length;

    private Hook hook;
    private RopeLink linkTarget;

    void Start()
    {
        length = transform.localScale.y;
    }

    void Update()
    {
        if (hook != null)
        {
            SetFront(hook.GetRopeAttachmentPoint());
        }
        else if (linkTarget != null)
        {
            SetFront(linkTarget.GetBack());
        }
    }

    public Vector3 GetFront()
    {
        var vec = transform.position + transform.up * (length / 2 + 0.1f);
        vec.z = z;
        return vec;
    }

    public Vector3 GetBack()
    {
        var vec= transform.position - transform.up * (length / 2 + 0.1f);
        vec.z = z;
        return vec;
    }

    public void SetTrack(Hook hook)
    {
        this.hook = hook;
        var look = hook.transform.position - transform.position;
        look.z = 0;
        transform.up = look;
    }
    public void SetTrack(RopeLink linkTarget)
    {
        this.linkTarget = linkTarget;
        var look = linkTarget.transform.up;
        look.z = z;
        transform.up = look;
    }

    private void SetFront(Vector3 newFront)
    {
        var previous_back = GetBack();
        var offset = newFront - GetFront();
        var position = Vector3.Lerp(transform.position, transform.position + offset, 0.5f);
        position.z = z;
        transform.position = position;
        transform.up = GetFront() - previous_back;
    }

    public void SetFrontAndBack(Vector3 front, Vector3 back)
    {
        var position = (front + back) / 2;
        position.z = z;
        transform.position = position;
        transform.up = front - back;
    }
}
