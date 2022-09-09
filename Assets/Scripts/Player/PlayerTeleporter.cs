using UnityEngine;

public class PlayerTeleporter : PlayerWeapon
{
    [SerializeField] GameObject player;
    [SerializeField] float fireSpeed;
    [SerializeField] GameObject teleporterBulletPrefab;
    [SerializeField] float recoilMagnitude;

    private GameObject teleporterBullet = null;

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

        if (!teleporterBullet) ShootTeleporterBullet();
        else TeleportToBullet();
        
    }

    private void ShootTeleporterBullet()
    {
        teleporterBullet = Instantiate(teleporterBulletPrefab, firePoint.transform.position, firePoint.transform.rotation);

        MoveStraight bulletMovement = teleporterBullet.GetComponent<MoveStraight>();
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

        AudioManager.PlayClipNow("TeleporterShoot");
    }

    private void TeleportToBullet()
    {
        var position = teleporterBullet.transform.position;
        var velocity = teleporterBullet.GetComponent<Rigidbody2D>().velocity;

        Destroy(teleporterBullet);
        teleporterBullet = null;

        player.transform.position = position;
        player.GetComponent<Rigidbody2D>().velocity = velocity;
    }
}
