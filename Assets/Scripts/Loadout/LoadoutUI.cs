using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoadoutUI : MonoBehaviour
{
    [SerializeField] KeyCode LoadoutHotkey;
    [SerializeField] PlayerWeaponManager WeaponManager;
    //[SerializeField] GameObject PrimaryWeaponUIs;
    [SerializeField] GameObject SecondaryWeaponsUI;
    //[SerializeField] GameObject UtilityWeaponsUI;
    [SerializeField] GameObject WeaponSlotPrefab;

    //private List<LoadoutSlot> PrimarySlots = new List<LoadoutSlot>();
    private List<LoadoutSlot> SecondarySlots = new List<LoadoutSlot>();
    //private List<LoadoutSlot> UtilitySlots = new List<LoadoutSlot>();


    private List<GameObject> weaponDescriptions = new List<GameObject>();

    void Start()
    {
        /*
        foreach (GameObject weapon in WeaponManager.PrimaryWeapons)
        {
            CreateLoadoutSlot(weapon, PrimaryWeaponUIs, PrimarySlots);  
        }
        */

        foreach (GameObject weapon in WeaponManager.SecondaryWeapons)
        {
            CreateLoadoutSlot(weapon, SecondaryWeaponsUI, SecondarySlots);
        }

        /*
        foreach (GameObject weapon in WeaponManager.UtilityWeapons)
        {
            CreateLoadoutSlot(weapon, UtilityWeaponsUI, UtilitySlots);
        }
        */

        //PrimarySlots[0].Select();
        SecondarySlots[0].Select();
        //UtilitySlots[0].Select();

        gameObject.SetActive(false);

        // Weapon Descriptions
        foreach (Transform child in transform)
        {
            var description = child.gameObject.GetComponent<LoadoutDescriptionUI>();
            if (description != null) weaponDescriptions.Add(child.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(LoadoutHotkey))
        {
            ExitLoadout();
        }
    }

    public void SlotClicked(LoadoutSlot slot)
    {
        // Check if slot is primary
        /*
        var idx = PrimarySlots.IndexOf(slot);
        var previousIdx = WeaponManager.CurrentPrimary;
        if (idx != -1)
        {
            PrimarySlots[previousIdx].Deselect(); // UI to disallow clicks and show check mark
            PrimarySlots[idx].Select(); // UI to allow clicks and hide check mark
            WeaponManager.SetPrimaryWeapon(idx); // Add correct weapon component to player
            AudioManager.PlayClipNow("ChangeLoadoutWeapon");
            return;
        }
        */

        // Check if slot is secondary (if not primary)
        var idx = SecondarySlots.IndexOf(slot);
        var previousIdx = WeaponManager.CurrentSecondary;
        if (idx != -1)
        {
            SecondarySlots[previousIdx].Deselect();
            SecondarySlots[idx].Select();
            WeaponManager.SetSecondaryWeapon(idx);
            AudioManager.PlayClipNow("ChangeLoadoutWeapon");
            return;
        }

        /*
        // Check if slot is utility (if not primary)
        idx = UtilitySlots.IndexOf(slot);
        previousIdx = WeaponManager.CurrentUtility;
        if (idx != -1)
        {
            UtilitySlots[previousIdx].Deselect();
            UtilitySlots[idx].Select();
            WeaponManager.SetUtilityWeapon(idx);
            AudioManager.PlayClipNow("ChangeLoadoutWeapon");
            return;
        }
        */
    }

    private void CreateLoadoutSlot(GameObject weapon, GameObject weaponsUI, List<LoadoutSlot> slots)
    {
        // Make new loadout slot
        var w = Instantiate(WeaponSlotPrefab, weaponsUI.transform);

        // Get weapon type data (specifically sprite icon)
        var weaponType = weapon.GetComponent<PlayerWeaponType>();

        // Get slot component, intialize and save it
        var slot = w.GetComponent<LoadoutSlot>();
        slot.Init(weaponType.IconSprite, weaponType.IsLocked, weaponType.weaponName);
        slots.Add(slot);
    }

    public void ExitLoadout()
    {
        PauseManager.Instance.SetPauseState(false);
        gameObject.SetActive(false); 
        HideWeaponInfo();
    }

    public bool SomeChoiceExists()
    {
        return /*MultipleUnlocked(PrimarySlots) || */ MultipleUnlocked(SecondarySlots); // || MultipleUnlocked(UtilitySlots);
    }

    private bool MultipleUnlocked(List<LoadoutSlot> slots)
    {
        return slots.Count(x => !x.IsLocked()) > 1;
    }

    public void ShowWeaponInfo(string weaponName)
    {
        foreach(GameObject descriptionObject in weaponDescriptions)
        {
            var description = descriptionObject.GetComponent<LoadoutDescriptionUI>();
            descriptionObject.SetActive(description != null && description.WeaponName == weaponName);
        }
    }

    public void HideWeaponInfo()
    {
        var currentWeapon = SecondarySlots.Find(x => x.IsSelected()).GetWeaponName();
        ShowWeaponInfo(currentWeapon);
    }
}
