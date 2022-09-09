using UnityEngine;

public class PlayerWeaponType : MonoBehaviour
{
    public enum WeaponType
    {
        PRIMARY,        // Melee & reflection           (LEFT HAND) (LEFT MOUSE)
        SECONDARY,      // Gun & recoil                 (RIGHT HAND) (RIGHT MOUSE)
        UTILITY         // Auxilary movement & special  (?) (SPACE BAR)
    }

    public WeaponType Weapon;
    public Sprite IconSprite;
    public bool IsLocked = false;
    public string weaponName;
}
