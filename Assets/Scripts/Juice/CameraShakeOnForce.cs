using System;
using UnityEngine;

public class CameraShakeOnForce : MonoBehaviour
{
    [SerializeField] AnimationCurve ShakeMagnitudeCurve;
    [SerializeField] float Duration;
    [SerializeField] float MinRealtiveVelocityThreshold;
    [SerializeField] float MaxRealtiveVelocityExpected;

    // Impulses for the player are between 0 and 11 with 11 being the hardest I can run into a wall

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var normal = (collision.GetContact(0).point - (Vector2) transform.position).normalized;
        var normalRelativeVelocity = normal * Vector2.Dot(collision.relativeVelocity, normal);
        var impactRelativeVelocity = normalRelativeVelocity.magnitude;

        if (impactRelativeVelocity <= MinRealtiveVelocityThreshold) return;
        var collisionProportion = Math.Min(impactRelativeVelocity, MaxRealtiveVelocityExpected) / (MaxRealtiveVelocityExpected - MinRealtiveVelocityThreshold);
        var magnitude = ShakeMagnitudeCurve.Evaluate(collisionProportion);
        var cameraShake = Camera.main.GetComponent<CameraShake>();
        if (cameraShake)
        {
            cameraShake.ShakeOnce(Duration, magnitude);
        } 
        else
        {
            Debug.LogWarning("No camera shake instance can be found on the main camera.");
        }
        AudioManager.PlayClipNow("Hit Wall", collisionProportion * collisionProportion);
    }
}
