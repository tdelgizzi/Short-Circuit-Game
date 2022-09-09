using UnityEngine;
using Unity.Services.Core;
using UnityEngine.Analytics;

public class AnalyticsManager : MonoBehaviour
{
    public static bool IsAnalyticsEnabled = true;

    [SerializeField] bool DebugAnalytics = false;

    private void Awake()
    {
        EventBus.Subscribe<AnalyticsEvent>(OnAnalyticsEvent);    
    }

    async void Start()
    {
        await UnityServices.InitializeAsync();
    }

    private void OnAnalyticsEvent(AnalyticsEvent obj)
    {
        if (!IsAnalyticsEnabled) return;
        Analytics.CustomEvent(obj.GetEventName(), obj.GetPayload());
        if (DebugAnalytics) Debug.Log("[ANALYTICS] -> Publish " + obj.GetEventName() + " Event");
        if (obj.SendImmediately()) Flush();
    }

    private void OnDestroy()
    {
        Flush();
    }

    private void Flush()
    {
        Analytics.FlushEvents();
        if (DebugAnalytics) Debug.Log("[ANALYTICS] -> FLUSH EVENTS");
    }
}
