using UnityEngine;

public class PlayerRailgun : PlayerWeapon
{
    private float fireSpeed;
    private float recoilMagnitude;

    [SerializeField] GameObject bulletPrefab;

    [SerializeField] float tier1ChargeTime;
    [SerializeField] float tier1RecoilMagnitude;
    [SerializeField] float tier1FireSpeed;

    [SerializeField] float tier2ChargeTime;
    [SerializeField] float tier2RecoilMagnitude;
    [SerializeField] float tier2FireSpeed;

    [SerializeField] float tier3ChargeTime;
    [SerializeField] float tier3RecoilMagnitude;
    [SerializeField] float tier3FireSpeed;

    
    

    [SerializeField] KeyCode cancelKey = KeyCode.Mouse0;

    bool charging = false;
    float chargeStartTime = -100f;
    int tier = 0;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        float remainingCooldown = lastAttackTime + cooldownTime - Time.time;

        if (charging)
        {
            if (tier == 0 && Time.time > chargeStartTime + tier1ChargeTime)
            {
                tier = 1;
                recoilMagnitude = tier1RecoilMagnitude;
                fireSpeed = tier1FireSpeed;
                animator.ResetTrigger("Fire");
                AudioManager.PlayClipNow("Railgun Charge 1");
            }
            else if (tier == 1 && Time.time > chargeStartTime + tier2ChargeTime)
            {
                tier = 2;
                recoilMagnitude = tier2RecoilMagnitude;
                fireSpeed = tier2FireSpeed;
                animator.ResetTrigger("Fire");
                AudioManager.PlayClipNow("Railgun Charge 2");
            }   
            else if (tier == 2 && Time.time > chargeStartTime + tier3ChargeTime)
            {
                tier = 3;
                recoilMagnitude = tier3RecoilMagnitude;
                fireSpeed = tier3FireSpeed;
                animator.ResetTrigger("Fire");
                AudioManager.PlayClipNow("Railgun Charge 3");
            }

            
            //animator.SetInteger("Tier", tier);

            if (Input.GetKeyDown(cancelKey))
            {
                tier = 0;
                lastAttackTime = Time.time;
                charging = false;
            }
        }
        animator.SetInteger("Tier", tier);

        if (Input.GetKeyDown(triggerKey) && !PauseManager.Instance.IsPaused())
        {
            if (!charging && remainingCooldown <= 0)
            {
                lastAttackTime = Time.time;
                chargeStartTime = Time.time;
                charging = true;
            }
        }
        else if (charging && !Input.GetKey(triggerKey) && !PauseManager.Instance.IsPaused())
        {
            charging = false;

            if (tier > 0)
            {
                Attack();
            }

            tier = 0;
            lastAttackTime = Time.time;
        }
    }

    protected override void Attack()
    {
        base.Attack();

        GameObject bullet = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);

        AltersHealth damage = bullet.GetComponent<AltersHealth>();
        damage.SetAmount(-1 * tier);

        MoveStraight bulletMovement = bullet.GetComponent<MoveStraight>();
        if (bulletMovement)
        {
            bulletMovement.SetSpeed(fireSpeed);
        }

        Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();
        if (rb)
        {
            rb.AddForce(-transform.up * recoilMagnitude, ForceMode2D.Impulse);
        }

        if (animator)
        {
            animator.SetTrigger("Fire");
            animator.SetInteger("Tier", tier);
        }

        AudioManager.PlayClipNow("Railgun Shoot");
    }
}
