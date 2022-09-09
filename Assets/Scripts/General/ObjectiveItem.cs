using UnityEngine;

public class ObjectiveItem : OrderedCollision
{
    public override void OrderedOnTriggerEnter(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            var inventory = other.gameObject.GetComponent<Inventory>();
            inventory.AddBattery(gameObject);
            AudioManager.PlayClipAtPoint(transform.position, "Battery Pickup");
            EventBus.PublishAnalyticsEvent(new ObjectiveCollectedAnalyticsEvent(
                other.transform.position,
                GameManager.Instance.GetTimeRemaining(),
                GameManager.Instance.GetCurrentCycle(),
                GameManager.Instance.GetRobots()
                ));
            gameObject.SetActive(false);
        }
    }
}
