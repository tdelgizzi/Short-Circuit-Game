using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject WeaponsRoom;
    private Collider weaponsRoomCollider;

    public class InventoryItem
    {
        public Sprite Sprite { get; set; }
        public string Name { get; set; }
        public GameObject Object { get; set; }
    }

    private List<InventoryItem> batteries = new List<InventoryItem>();
    private List<InventoryItem> weapons = new List<InventoryItem>();

    private void Start()
    {
        weaponsRoomCollider = WeaponsRoom.GetComponent<BoxCollider>();
    }

    private void Update()
    {
        // We have returned to the weapons room!
        if (weaponsRoomCollider.bounds.Contains(transform.position))
        {
            Success();
            GameManager.Instance.ResetTime();
        }
    }

    public void AddWeapon(GameObject weapon)
    {
        var sprite = weapon.GetComponent<SpriteRenderer>()?.sprite;
        if (!sprite)
        {
            Debug.LogWarning("[Inventory] Weapon object has no sprite to render.");
            return;
        }

        var collectableWeapon = weapon.GetComponent<CollectableWeapon>();
        if (!collectableWeapon)
        {
            Debug.LogWarning("[Inventory] Weapon object has no collectableWeapon component and therefore no name.");
            return;
        }
        var name = collectableWeapon.WeaponName;

        weapons.Add(new InventoryItem
        {
            Sprite = sprite,
            Name = name,
            Object = weapon
        });

        UpdateUI();
    }

    public void AddBattery(GameObject battery)
    {
        batteries.Add(new InventoryItem
        {
            Sprite = null,
            Name = "Battery",
            Object = battery
        });

        UpdateUI();
    }

    public void Success()
    {
        // Deal with weapons
        foreach (InventoryItem weapon in weapons)
        {
            EventBus.Publish(new UnlockWeaponEvent(weapon.Name));
            Destroy(weapon.Object);
        }
        weapons.Clear();

        // Deal with batteries
        foreach (InventoryItem battery in batteries)
        {
            GameManager.Instance.IncrementScore();
            Destroy(battery.Object);
        }
        batteries.Clear();

        UpdateUI();
    }

    public void Failure(Vector3 deathPosition)
    {
        foreach (InventoryItem weapon in weapons)
        {
            //weapon.Object.transform.position = deathPosition;
            //weapon.Object.SetActive(true);
            DropItem(weapon.Object, deathPosition);
        }
        weapons.Clear();

        // Deal with batteries
        foreach (InventoryItem battery in batteries)
        {
            //battery.Object.transform.position = deathPosition;
            //battery.Object.SetActive(true);
            DropItem(battery.Object, deathPosition);
        }
        batteries.Clear();

        UpdateUI();
    }

    private void DropItem(GameObject item, Vector3 deathPosition)
    {
        var position = (Vector3) Random.insideUnitCircle + deathPosition;
        item.transform.position = position;
        item.SetActive(true);
    }

    private List<InventoryItem> GetItems()
    {
        return weapons.Concat(batteries).ToList();
    }

    private void UpdateUI()
    {
        DisplayManager.Instance.UpdateInventoryUI(GetItems());
    }

    public string[] GetItemNames()
    {
        return weapons.Select(w => w.Name).ToArray();
    }
}
