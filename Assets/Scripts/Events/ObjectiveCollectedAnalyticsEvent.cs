using System.Collections.Generic;
using UnityEngine;

public class ObjectiveCollectedAnalyticsEvent : AnalyticsEvent
{
    public enum Reason
    {
        Shot,
        Time
    }

    private Vector2 location;
    private float time;
    private int cycle;
    private int robotsRemaining;

    public ObjectiveCollectedAnalyticsEvent(Vector2 location, float time, int cycle, int robotsRemaining)
    {
        this.location = location;
        this.time = time;
        this.cycle = cycle;
        this.robotsRemaining = robotsRemaining;
    }

    public override string GetEventName()
    {
        return "OBJECTIVE_COLLECTED";
    }

    public override Dictionary<string, object> GetPayload()
    {
        return new Dictionary<string, object>
        {
            { "location", location },
            { "time", time },
            { "cycle", cycle },
            { "numRobotsRemaining", robotsRemaining }
        };
    }

    public override bool SendImmediately()
    {
        return false;
    }
}

