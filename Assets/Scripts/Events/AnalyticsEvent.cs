using System.Collections.Generic;

public abstract class AnalyticsEvent
{
    public abstract string GetEventName();

    public abstract Dictionary<string, object> GetPayload();

    public abstract bool SendImmediately();
}
