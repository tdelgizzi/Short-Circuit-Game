using UnityEngine;

public class PlayerWallHitDetector : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            EventBus.PublishAnalyticsEvent(new PlayerHitWallAnalyticsEvent(transform.position));
        }
    }
}
