using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class BiterEnemyLogic : EnemyLogic {
    [SerializeField] GameObject pincers;
    [SerializeField] float pursueSpeed = 1.0f;
    [SerializeField] float biteFrequency;

    Animator animator;
    BiterAnimationEvent animationEvent;
    Collider2D pincerCollider;

    private EnemyActions currentAction = EnemyActions.Patrolling;
    private Vector3 targetPosition = Vector3.zero;
    private bool biting = false;
    private float lastBiteTime;

    Vector3 goingToPoint;
    float lastGoTime;
    float goLength;

    protected override void Start() {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        animator = pincers.GetComponent<Animator>();
        animationEvent = pincers.GetComponentInChildren<BiterAnimationEvent>();
        pincerCollider = pincers.GetComponent<Collider2D>();
        lastBiteTime = Time.time;
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
                if (!biting || Time.time - lastBiteTime > biteFrequency) {
                    lastBiteTime = Time.time;
                    StartCoroutine(BiteCoroutine());
                }
                break;

            case EnemyActions.Pursuing:
                if (IsCloseEnoughToPosition(targetPosition)) {
                    currentAction = EnemyActions.Patrolling;
                }
                break;
        }
    }

    IEnumerator BiteCoroutine() {
        biting = true;
        animator?.SetTrigger("Bite");

        yield return new WaitUntil(() => animationEvent.state == BiterAnimationEvent.BiteAnimationState.Snap);
        rb.AddForce(this.transform.up * movementSpeed, ForceMode2D.Impulse);

        pincerCollider.enabled = true;

        AudioManager.PlayClipAtPoint(this.transform.position, "BiterBite");

        yield return new WaitUntil(() => animationEvent.state == BiterAnimationEvent.BiteAnimationState.Shut);
        rb.AddForce(-this.transform.up * movementSpeed * 0.05f, ForceMode2D.Impulse);

        pincerCollider.enabled = false;

        yield return new WaitUntil(() => animationEvent.state == BiterAnimationEvent.BiteAnimationState.Done);

        biting = false;
    }

    protected override void OnVisionEnter(Transform t) {
        base.OnVisionEnter(t);
        targetPosition = t.position;
        currentAction = EnemyActions.Attacking;
    }

    protected override void OnVisionStay(Transform t) {
        base.OnVisionStay(t);
        SmoothLookAt(targetPosition);
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
