using UnityEngine;

public class CollectableWeapon : MonoBehaviour
{
    public string WeaponName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            var inventory = collision.gameObject.GetComponent<Inventory>();
            inventory.AddWeapon(gameObject);
            AudioManager.PlayClipAtPoint(transform.position, "ItemPickup");
            gameObject.SetActive(false);
        }
    }
}
