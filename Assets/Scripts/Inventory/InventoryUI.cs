using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Inventory;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Sprite BatterySprite;

    private List<GameObject> icons = new List<GameObject>();

    void Start()
    {
        SetWidth(0);
    }

    public void UpdateItems(List<InventoryItem> items)
    {
        SetWidth(items.Count);
        FitIconsToSize(items.Count);
        SetSprites(items);
        SetVisible(items.Count);
    }

    private void SetSprites(List<InventoryItem> items) { 
        for (int i = 0; i < icons.Count; i++)
        {
            var sprite = items[i].Sprite;
            if (items[i].Name == "Battery") sprite = BatterySprite;
            icons[i].GetComponent<Image>().sprite = sprite;
        }
    }

    private void FitIconsToSize(int count)
    {
        if (icons.Count > count)
        {
            for (int i = count; i < icons.Count; i++)
            {
                Destroy(icons[i]);
            }
            icons = icons.Take(count).ToList();
        }
        else if (icons.Count < count)
        {
            for (int i = icons.Count; i < count; i++)
            {
                icons.Add(SpawnSprite());
            } 
        }
    }

    private void SetVisible(int count)
    {
        transform.parent.gameObject.SetActive(count != 0);
    }

    private void SetWidth(int count)
    {
        var rt = GetComponent<RectTransform>();
        var size = rt.sizeDelta;

        // If count > 1, size is 150 or greater
        size.x = Mathf.Max(40 + count * 60, 150);
        if (count == 0) size.x = 0;

        rt.sizeDelta = size;
    }

    private GameObject SpawnSprite()
    {
        var obj = new GameObject("Inventory Slot");
        obj.transform.SetParent(transform);
        obj.AddComponent<Image>();
        return obj;
    }
}
