using UnityEngine;
using UnityEngine.UI;

public class LoadoutSlot : MonoBehaviour
{
    [SerializeField] GameObject WeaponSprite;
    [SerializeField] GameObject Checkbox;
    [SerializeField] GameObject LockedImage;

    private LoadoutUI loadoutUI;
    private Button button;
    private CanvasGroup canvasGroup;

    private string weaponName;
    private bool isLocked = false;

    private bool selected = false;

    void Awake()
    {
        loadoutUI = transform.GetComponentInParent<LoadoutUI>();
        button = GetComponent<Button>();
        canvasGroup = GetComponent<CanvasGroup>();

        button.onClick.AddListener(OnClick);

        EventBus.Subscribe<UnlockWeaponEvent>(OnWeaponUnlock);
    }

    private void OnWeaponUnlock(UnlockWeaponEvent obj)
    {
        if (obj.WeaponName == weaponName) Unlock();
    }

    public void Init(Sprite sprite, bool isLocked, string weaponName)
    {
        WeaponSprite.GetComponent<Image>().sprite = sprite;

        if (isLocked) Lock();
        else Unlock();

        this.weaponName = weaponName;
    }

    private void OnClick()
    {
        loadoutUI.SlotClicked(this);
    }

    private void Lock()
    {
        isLocked = true;
        button.enabled = false;
        LockedImage.SetActive(true);
        WeaponSprite.SetActive(false);
        canvasGroup.alpha = 0.5f;
    }

    public void Unlock()
    {
        isLocked = false;
        button.enabled = true;
        LockedImage.SetActive(false);
        WeaponSprite.SetActive(true);
        canvasGroup.alpha = 1.0f;
    }

    public void Select()
    {
        selected = true;
        button.enabled = false;
        Checkbox.SetActive(true);
    }

    public void Deselect()
    {
        selected = false;
        button.enabled = true;
        Checkbox.SetActive(false);
    }

    public void MouseEnter()
    {
        loadoutUI.ShowWeaponInfo(weaponName);
    }

    public void MouseExit()
    {
        loadoutUI.HideWeaponInfo();
    }

    public bool IsLocked()
    {
        return isLocked;
    }

    public bool IsSelected()
    {
        return selected;
    }

    public string GetWeaponName()
    {
        return weaponName;
    }
}
