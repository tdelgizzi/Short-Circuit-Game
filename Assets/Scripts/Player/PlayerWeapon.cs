using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    // A gameobject to initate weapon calculation on that works
    // indepently of arm position.
    // i.e. Location for bullet spawn, melee center
    [SerializeField] protected GameObject firePoint;

    // [REQUIRED] Keycode to "Fire the weapon" 
    public KeyCode triggerKey = KeyCode.Mouse0;

    // Time between attacks
    public float cooldownTime = 1f;
    protected float lastAttackTime = -100f;

    protected Animator animator;

    // This should be in a parent object for the loadout system
    private PlayerWeaponType weaponType;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        weaponType = transform.GetComponentInParent<PlayerWeaponType>();
    }

    protected virtual void Update()
    {
        float remainingCooldown = lastAttackTime + cooldownTime - Time.time;

        if (Input.GetKeyDown(triggerKey) && !PauseManager.Instance.IsPaused())
        {
            if (remainingCooldown <= 0)
            {
                lastAttackTime = Time.time;
                Attack();
            }
        }
    }

    protected virtual void Attack()
    {
        EventBus.PublishAnalyticsEvent(new WeaponUsedAnalyticsEvent(weaponType.weaponName));
    }
}
