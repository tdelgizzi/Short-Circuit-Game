using UnityEngine;

public class PlayerPistol : PlayerWeapon
{
    [SerializeField] float fireSpeed;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float recoilMagnitude;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Attack()
    {
        base.Attack();

        GameObject bullet = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);

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
        }

        AudioManager.PlayClipNow("Shoot");
    }
}
