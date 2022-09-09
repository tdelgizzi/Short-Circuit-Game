using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CanvasElementAlwaysUpright : MonoBehaviour
{
    private RectTransform rt;

    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        rt.up = Vector3.up;
    }
}
