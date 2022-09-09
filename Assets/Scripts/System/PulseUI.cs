using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class PulseUI : MonoBehaviour
{
    [SerializeField] float minScaleFactor = 0.9f;
    [SerializeField] float maxScaleFactor = 1.0f;
    [SerializeField] float pulseFrequency = 1.0f;

    Vector2 initialScale = Vector2.one;

    private RectTransform rt;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        initialScale = rt.localScale;
    }

    private void Update()
    {
        var amplitude = maxScaleFactor - minScaleFactor;
        rt.localScale = (Mathf.Sin(Time.time * pulseFrequency) * amplitude + minScaleFactor) * initialScale;
    }
}
