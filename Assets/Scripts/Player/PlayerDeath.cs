using System.Collections.Generic;
using UnityEngine;
using static PlayerDiedAnalyticsEvent;

public class PlayerDeath : MonoBehaviour
{
    public static Dictionary<Reason, string> DeathExplanations = new Dictionary<Reason, string>() {
        {  Reason.Shot, "Your robot took too much damage from enemies" },
        {  Reason.Time, "Your robot ran out of battery" }
    };

    [SerializeField] GameObject DeadRobotPrefab;

    public void Die(Reason reason)
    {
        // Get inventory item names if they exists
        var inventory = GetComponent<Inventory>();
        string[] inventoryItemNames = new string[0];
        if (inventory) inventoryItemNames = inventory.GetItemNames();

        // Publish Events
        EventBus.PublishAnalyticsEvent(new PlayerDiedAnalyticsEvent(transform.position, reason, inventoryItemNames));

        // Capture physics
        var position = transform.position;
        var rotation = transform.rotation;
        var velocity = GetComponent<Rigidbody2D>().velocity;

        // Deactivate player object
        gameObject.SetActive(false);

        // Create dead robot body
        var deadRobot = Instantiate(DeadRobotPrefab, position, rotation);
        deadRobot.GetComponent<Rigidbody2D>().velocity = velocity;

        // Empty inventory onto floor
        GetComponent<Inventory>()?.Failure(position);

        // Pause to prevent timer from running
        PauseManager.Instance.SetPauseState(true);
        EventBus.Publish(new PlayerDiedEvent());

        // Show death screen and death reason
        GameManager.Instance.RemoveRobot();
        if (GameManager.Instance.GetRobots() < 0)
        {
            DisplayManager.Instance.UpdateLoseScreen(true);
        }
        else
        {
            DisplayManager.Instance.UpdateDieScreen(true, DeathExplanations[reason]);
        }
    }
}
