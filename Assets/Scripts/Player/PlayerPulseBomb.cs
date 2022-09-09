using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPulseBomb : PlayerWeapon
{
    [SerializeField] GameObject pulseBombObject;
    [SerializeField] float fireSpeed;


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

        GameObject bullet = Instantiate(pulseBombObject, firePoint.transform.position, firePoint.transform.rotation);

        MoveStraight bulletMovement = bullet.GetComponent<MoveStraight>();
        if (bulletMovement)
        {
            bulletMovement.SetSpeed(fireSpeed);
        }

        AudioManager.PlayClipNow("Railgun Charge 2");
    }
}
