using System.Collections.Generic;
using UnityEngine;

public class PlayerHitWallAnalyticsEvent : AnalyticsEvent
{
    public enum Reason
    {
        Shot,
        Time
    }

    private Vector2 location;

    public PlayerHitWallAnalyticsEvent(Vector2 location)
    {
        this.location = location;
    }

    public override string GetEventName()
    {
        return "PLAYER_HIT_WALL";
    }

    public override Dictionary<string, object> GetPayload()
    {
        return new Dictionary<string, object>
        {
            { "location", location }
        };
    }

    public override bool SendImmediately()
    {
        return false;
    }
}


