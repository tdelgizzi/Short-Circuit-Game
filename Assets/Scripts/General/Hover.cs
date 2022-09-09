using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour {

    [SerializeField] float hoverDistance;
    [SerializeField] float hoverForce;
    [SerializeField] int hoverGranularity;

    int layerMask;
    Rigidbody2D rb;
    Collider2D col;

    void Start() {
        Physics2D.queriesStartInColliders = false;
        layerMask = LayerMask.GetMask("Wall");
        rb = this.GetComponent<Rigidbody2D>();
        col = this.GetComponent<Collider2D>();
    }

    void FixedUpdate() {
        // Raycast <hoverGranularity> times in a circle
        for (int i = 0; i < hoverGranularity; ++i) {
            Vector3 castDirection = Quaternion.AngleAxis(360 / hoverGranularity * i, Vector3.forward) * Vector3.up;
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, castDirection, hoverDistance, layerMask);
            if (hit) {
                float distance = (col.ClosestPoint(hit.point) - hit.point).magnitude;
                Vector3 forceDirection = hit.normal;
                float velocityFactor = Mathf.Abs(Mathf.Min(Vector3.Dot(rb.velocity, forceDirection), 0));
                float forceMultiplier = 1f - distance / hoverDistance;

                rb.AddForce(forceDirection * hoverForce * forceMultiplier * forceMultiplier * velocityFactor);
                Debug.DrawLine(col.ClosestPoint(hit.point), col.ClosestPoint(hit.point) + (Vector2) forceDirection * hoverForce * forceMultiplier * velocityFactor, Color.red, 1f);
            }
        }        
    }
}
