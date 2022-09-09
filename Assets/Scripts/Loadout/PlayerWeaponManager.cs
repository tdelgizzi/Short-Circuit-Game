using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField] Transform LeftArm;
    [SerializeField] Transform RightArm;
    [SerializeField] Transform Player;

    public List<GameObject> PrimaryWeapons { get; private set; }
    public List<GameObject> SecondaryWeapons { get; private set; }
    public List<GameObject> UtilityWeapons { get; private set; }

    public int CurrentPrimary { get; private set; } = 0;
    public int CurrentSecondary { get; private set; } = 0;
    public int CurrentUtility { get; private set; } = 0;


    // TODO: utility

    void Awake()
    {
        // Find all weapons
        PrimaryWeapons = GetWeaponsOfType(PlayerWeaponType.WeaponType.PRIMARY);
        SecondaryWeapons = GetWeaponsOfType(PlayerWeaponType.WeaponType.SECONDARY);
        UtilityWeapons = GetWeaponsOfType(PlayerWeaponType.WeaponType.UTILITY);

        // Set deafult weapons
        SetWeapon(PrimaryWeapons[0], LeftArm, true);
        SetWeapon(SecondaryWeapons[0], RightArm, true);
        SetWeapon(UtilityWeapons[0], Player, true);
    }

    private List<GameObject> GetWeaponsOfType(PlayerWeaponType.WeaponType weaponType)
    {
        var weapons = new List<GameObject>();
        foreach (Transform child in transform)
        {
            var playerWeaponType = child.gameObject.GetComponent<PlayerWeaponType>();
            if (playerWeaponType == null) continue;
            if (playerWeaponType.Weapon == weaponType) weapons.Add(child.gameObject);
        }
        return weapons;
    }

    public void SetPrimaryWeapon(int idx)
    {
        // Remove previous primary
        SetWeapon(PrimaryWeapons[CurrentPrimary], transform, false);

        // Set new primary weapon
        SetWeapon(PrimaryWeapons[idx], LeftArm, true);

        // Set index
        CurrentPrimary = idx;
    }

    public void SetSecondaryWeapon(int idx)
    {
        // Remove previous secondary
        SetWeapon(SecondaryWeapons[CurrentSecondary], transform, false);

        // Set new secondary weapon
        SetWeapon(SecondaryWeapons[idx], RightArm, true);

        // Set index
        CurrentSecondary = idx;
    }

    public void SetUtilityWeapon(int idx)
    {
        // Remove previous utility
        SetWeapon(UtilityWeapons[CurrentUtility], transform, false);

        // Set new utility weapon
        SetWeapon(UtilityWeapons[idx], Player, true);

        // Set index
        CurrentUtility = idx;
    }

    private void SetWeapon(GameObject weapon, Transform parent, bool active)
    {
        weapon.transform.SetParent(parent);
        weapon.transform.localPosition = Vector2.zero;
        weapon.transform.localRotation = Quaternion.identity;
        weapon.SetActive(active);
    }

    private void OnDestroy()
    {
        DestroyWeapons(PrimaryWeapons);
        DestroyWeapons(SecondaryWeapons);
        DestroyWeapons(UtilityWeapons);
    }

    private void DestroyWeapons(List<GameObject> weapons)
    {
        foreach (GameObject weapon in weapons) Destroy(weapon);
    }
}
