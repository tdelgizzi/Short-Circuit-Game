using UnityEngine;

public class InventoryTitleUI : MonoBehaviour
{
    [SerializeField] RectTransform inventory;

    private RectTransform rt;

    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        var x = inventory.sizeDelta.x / -2f;
        rt.anchoredPosition = new Vector2(x, rt.anchoredPosition.y);
    }
}
