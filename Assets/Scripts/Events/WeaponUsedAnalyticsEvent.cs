using System.Collections.Generic;

public class WeaponUsedAnalyticsEvent : AnalyticsEvent
{
    public string WeaponName;

    public WeaponUsedAnalyticsEvent(string weaponName)
    {
        WeaponName = weaponName;
    }

    public override string GetEventName()
    {
        return "WEAPON_USED";
    }

    public override Dictionary<string, object> GetPayload()
    {
        return new Dictionary<string, object>()
        {
            { "weaponName", WeaponName }
        };
    }

    public override bool SendImmediately()
    {
        return false;
    }
}
