using System.Collections.Generic;
using UnityEngine;

public class PlayerDiedAnalyticsEvent : AnalyticsEvent
{
    public enum Reason
    {
        Shot,
        Time
    }

    private Vector2 location;
    private Reason reason;
    private string[] inventoryItemNames;

    public PlayerDiedAnalyticsEvent(Vector2 location, Reason reason, string[] inventoryItemNames)
    {
        this.location = location;
        this.reason = reason;
        this.inventoryItemNames = inventoryItemNames;
    }

    public override string GetEventName()
    {
        return "PLAYER_DIED_EVENT";
    }

    public override Dictionary<string, object> GetPayload()
    {
        return new Dictionary<string, object>
        {
            { "location", location },
            { "reason", reason.ToString() },
            { "inventory", string.Join(",", inventoryItemNames) }
        };
    }

    public override bool SendImmediately()
    {
        return true;
    }
}
