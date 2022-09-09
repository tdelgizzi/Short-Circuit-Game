using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : PlayerWeapon
{
    [SerializeField] LayerMask attackMask;
    [SerializeField] LayerMask wallMask;
    [SerializeField] LayerMask physicsMask;
    [SerializeField] int alterHealthAmount = -1;
    [SerializeField] float impactForceMagnitude = 300f;
    public float radius = 2.1f; // Hardcoded to animation
    float angle = 111f; // Hardcoded to animation
    [SerializeField] float duration = 0.16f;

    bool meleeInProgress = false;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void OnEnable()
    {
        meleeInProgress = false;
    }

    protected override void Attack()
    {
        if (meleeInProgress) return;

        base.Attack();

        StartCoroutine(MeleeAttack());

        AudioManager.PlayClipNow("Melee");
    }


    IEnumerator MeleeAttack()
    {
        meleeInProgress = true;
        animator.SetTrigger("Fire");

        var endTime = Time.time + duration;
        List<Transform> completedTargets = new List<Transform>();

        while (Time.time <= endTime)
        {
            AttackTargets(completedTargets);
            yield return null;
        }

        /*
        for (int i = 0; i < iterations; i++)
        {
            yield return new WaitForSeconds(i / iterations);
        }
        */

        animator.ResetTrigger("Fire");
        meleeInProgress = false;
    }

    private void AttackTargets(List<Transform> completedTargets)
    {
        // Attackable Entities
        List<Transform> attackTargets = GetTargets(attackMask, completedTargets);

        foreach (Transform t in attackTargets)
        {
            HasHealth otherHealth = t.gameObject.GetComponent<HasHealth>();
            if (otherHealth)
            {
                otherHealth.UpdateHealth(alterHealthAmount);
            }

            completedTargets.Add(t);
        }

        // Targetable Physical Objects
        List<Transform> physicalTargets = GetTargets(physicsMask, completedTargets);

        foreach (Transform t in physicalTargets)
        {
            Rigidbody2D rb = t.gameObject.GetComponent<Rigidbody2D>();
            if (rb)
            {
                var forceDirection = (t.transform.position - transform.position).normalized;
                rb.AddForce(forceDirection * impactForceMagnitude);
            }

            MoveStraight ms = t.gameObject.GetComponent<MoveStraight>();
            if (ms)
            {
                Vector2 newT = (t.transform.position - transform.position).normalized;
                t.up = newT;
            }

            CollisionOrdering collisionOrdering = t.gameObject.GetComponent<CollisionOrdering>();
            if (collisionOrdering)
            {
                collisionOrdering.sourceObject = this.gameObject;
            }

            ChangeLayerOnReflection clor = t.gameObject.GetComponent<ChangeLayerOnReflection>();
            if (clor)
            {
                clor.ChangeToLayer();
            }

            completedTargets.Add(t);
        }
    }

    List<Transform> GetTargets(LayerMask targetMask, List<Transform> ignoreList)
    {
        List<Transform> attackTargets = new List<Transform>();

        Collider2D[] targetsInViewRadius =
            Physics2D.OverlapCircleAll(firePoint.transform.position, radius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - firePoint.transform.position).normalized;
            if (Vector3.Angle(firePoint.transform.up, dirToTarget) < angle / 2)
            {
                float dstToTarget = Vector3.Distance(firePoint.transform.position, target.position);

                if (!Physics2D.Raycast(firePoint.transform.position, dirToTarget, dstToTarget, wallMask))
                {
                    if (!ignoreList.Contains(target))
                        attackTargets.Add(target);
                }
            }
            else
            {
                int fullMask = wallMask.value | attackMask.value;
                RaycastHit2D hit = Physics2D.Raycast(firePoint.transform.position, DirFromAngle(angle / 2, false), radius, fullMask);
                if (hit.collider != null)
                {
                    if (hit.collider.Equals(targetsInViewRadius[i]))
                        if (!ignoreList.Contains(target))
                            attackTargets.Add(target);
                }
            }
        }

        return attackTargets;
    }

    public Vector3 DirFromAngle(float angle, bool globalAngle)
    {
        if (!globalAngle)
            angle += -transform.eulerAngles.z;

        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad),
                           Mathf.Cos(angle * Mathf.Deg2Rad),
                           0);
    }
}
