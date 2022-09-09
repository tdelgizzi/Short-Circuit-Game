using System.Collections.Generic;

public class GameWonAnalyticsEvent : AnalyticsEvent
{
    private int numRuns = 0;
    private int robotsRemaining = 0;

    public GameWonAnalyticsEvent(int numRuns, int robotsRemaining)
    {
        this.numRuns = numRuns;
        this.robotsRemaining = robotsRemaining;
    }

    public override string GetEventName()
    {
        return "GAME_WON";
    }

    public override Dictionary<string, object> GetPayload()
    {
        return new Dictionary<string, object>
        {
            { "numRuns", numRuns },
            { "numRobotsRemaining", robotsRemaining }
        };
    }

    public override bool SendImmediately()
    {
        return true;
    }
}
