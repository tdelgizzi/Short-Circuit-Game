using System.Collections.Generic;

public class GameLostAnalyticsEvent : AnalyticsEvent
{
    private int numRuns = 0;
    private int robotsRemaining = 0;
    private int score = 0;

    public GameLostAnalyticsEvent(int numRuns, int robotsRemaining, int score)
    {
        this.numRuns = numRuns;
        this.robotsRemaining = robotsRemaining;
        this.score = score;
    }

    public override string GetEventName()
    {
        return "GAME_LOST";
    }

    public override Dictionary<string, object> GetPayload()
    {
        return new Dictionary<string, object>
        {
            { "numRuns", numRuns },
            { "numRobotsRemaining", robotsRemaining },
            { "score", score }
        };
    }

    public override bool SendImmediately()
    {
        return true;
    }
}
