using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    // Set by the grapple manager
    public GameObject Player { get; set; }

    private bool hooked = false;
    private bool onMovingObject = false;

    private Rigidbody2D rb;
    private HingeJoint2D hinge;

    private GameObject target;
    private Vector3 offset;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hinge = GetComponent<HingeJoint2D>();
    }

    void Update()
    {
        if (hooked && !onMovingObject) {
            // transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, 0.1f);
            rb.velocity = Vector2.zero;
        }

        // if (IsOutsideOfCamera())
        //{
        // EventBus.Publish(new GrappleOutsideOfCameraEvent());
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hooked) return;

        hooked = true;
        target = collision.gameObject;
        offset = transform.position - target.transform.position;
        rb.freezeRotation = true;

        Rigidbody2D otherRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (otherRb) {
            hinge.enabled = true;
            hinge.connectedBody = otherRb;
            onMovingObject = true;
        } else {
            onMovingObject = false;
        }
        
        EventBus.Publish(new GrappleHitTargetEvent { Target = target });
        AudioManager.PlayClipAtPoint(transform.position, "Impact", 0.5f);
    }

    private bool IsOutsideOfCamera()
    {
        var bounds = OrthographicBounds(Camera.main);
        var rect = new Rect(bounds.min.x, bounds.min.y, bounds.max.x - bounds.min.x, bounds.max.y - bounds.min.y);
        return !rect.Contains(transform.position);
    }

    // Found: https://answers.unity.com/questions/501893/calculating-2d-camera-bounds.html
    public Bounds OrthographicBounds(Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }
}
