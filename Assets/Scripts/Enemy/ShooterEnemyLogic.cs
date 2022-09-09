using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class ShooterEnemyLogic : EnemyLogic {
    [SerializeField] GameObject firePoint;
    [SerializeField] GameObject enemyProjectilePrefab;
    [SerializeField] float shootCooldownTime = 2f;

    private EnemyActions currentAction = EnemyActions.Patrolling;
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 targetVelocity = Vector3.zero;
    Animator animator;

    float lastShotTime = -100f;
    bool currentlyShooting = false;

    Vector3 goingToPoint;
    float lastGoTime;
    float goLength;
    
    protected override void Start() {
        base.Start();
        animator = this.GetComponent<Animator>();
    }

    protected override void Update() {
        base.Update();
        switch (currentAction) {
            case EnemyActions.Patrolling:
                // Maybe eventually have some neat patrolling behavior, right now just sit tight little bitey
                if (Time.time - lastGoTime > goLength) {
                    goingToPoint = this.transform.position + (Vector3) (Random.insideUnitCircle * Random.Range(2.0f, 8.0f));

                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(goingToPoint, out hit, 1.0f, NavMesh.AllAreas)) {
                        goingToPoint = hit.position;
                    }
                    
                    MoveTowards(goingToPoint);

                    lastGoTime = Time.time;
                    goLength = Random.Range(1.0f, 5.0f);
                } else {
                    SmoothLookAt(destination);
                }
                break;

            case EnemyActions.Attacking:
                if ((targetPosition - transform.position).magnitude > 5) {
                    MoveTowards(targetPosition);
                    SmoothLookAt(targetPosition);
                }

                float timeSinceLastShot = Time.time - lastShotTime;

                // Vector3 dir = targetPosition + targetVelocity - transform.position;
                // dir = transform.InverseTransformDirection(dir);
                // float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

                if (timeSinceLastShot > shootCooldownTime && !currentlyShooting) {
                    StartCoroutine(bulletChargeAndShoot());
                }
                break;

            case EnemyActions.Pursuing:
                if (IsCloseEnoughToPosition(targetPosition)) {
                    currentAction = EnemyActions.Searching;
                }
                break;

            case EnemyActions.Searching:
                break;
        }
    }

    IEnumerator bulletChargeAndShoot() {
        currentlyShooting = true;
        animator?.SetTrigger("Charge");

        float chargeTimeStart = Time.time;
        while (Time.time - chargeTimeStart < 1.0f) {
            SmoothLookAt(targetPosition);
            yield return null;
        }

        animator?.SetTrigger("Fire");
        AudioManager.PlayClipAtPoint(transform.position, "Enemy Shoot");
        
        GameObject proj = GameObject.Instantiate(enemyProjectilePrefab, firePoint.transform.position, firePoint.transform.rotation);
        CollisionOrdering collisionOrdering = proj.GetComponent<CollisionOrdering>();

        if (collisionOrdering)
            collisionOrdering.sourceObject = this.gameObject;
        
        lastShotTime = Time.time;
        currentlyShooting = false;
    }

    protected override void OnVisionEnter(Transform t) {
        base.OnVisionEnter(t);
        targetPosition = t.position;
        targetVelocity = t.GetComponent<Rigidbody2D>().velocity;
        currentAction = EnemyActions.Attacking;
    }

    protected override void OnVisionStay(Transform t) {
        base.OnVisionStay(t);
        targetVelocity = t.GetComponent<Rigidbody2D>().velocity;
        targetPosition = t.position;
        if (currentAction != EnemyActions.Attacking) {
            currentAction = EnemyActions.Attacking;
        }
    }

    protected override void OnVisionExit(Transform t) {
        base.OnVisionExit(t);
        currentAction = EnemyActions.Pursuing;
        MoveTowards(t.position);
    }
}
