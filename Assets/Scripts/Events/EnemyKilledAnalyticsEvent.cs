using UnityEngine;
using System.Collections.Generic;

public class EnemyKilledAnalyticsEvent : AnalyticsEvent
{
    private string enemyName;

    public EnemyKilledAnalyticsEvent(GameObject enemy)
    {
        enemyName = enemy.name;
        var spaceIndex = enemyName.IndexOf(' ');
        if (spaceIndex > 0) enemyName = enemyName.Substring(0, spaceIndex);
    }

    public override string GetEventName()
    {
        return "ENEMY_KILLED";
    }

    public override Dictionary<string, object> GetPayload()
    {
        return new Dictionary<string, object>()
        {
            { "enemyName", enemyName }
        };
    }

    public override bool SendImmediately()
    {
        return false;
    }
}
