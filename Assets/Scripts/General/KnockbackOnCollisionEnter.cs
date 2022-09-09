using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackOnCollisionEnter : MonoBehaviour {
    [SerializeField] LayerMask targetLayers;
    [SerializeField] float magnitude;

    void OnCollisionEnter2D(Collision2D other) {
        Rigidbody2D otherRb = other.gameObject.GetComponent<Rigidbody2D>();
        if (LayerMaskUtil.IsInLayerMask(other.gameObject.layer, targetLayers) && otherRb) {
            Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
            Vector3 direction = rb.velocity.normalized;
            otherRb.AddForceAtPosition(direction*magnitude, other.GetContact(0).point, ForceMode2D.Impulse);
        }
    }
}
