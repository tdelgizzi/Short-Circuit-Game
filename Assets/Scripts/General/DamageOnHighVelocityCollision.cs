using UnityEngine;

public class DamageOnHighVelocityCollision : MonoBehaviour
{
    [SerializeField] LayerMask layersToDamage;
    [SerializeField] int healthDelta;
    [SerializeField] float minVelocity = 7.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var other = collision.collider.gameObject;

        // Must be going fast enough
        if (collision.relativeVelocity.magnitude < minVelocity) return;

        // Must be in a selected layer
        if (layersToDamage != (layersToDamage | (1 << other.layer))) return;

        // Must have health
        HasHealth health = other.GetComponent<HasHealth>();
        if (!health) return;

        health.UpdateHealth(healthDelta);
    }
}
