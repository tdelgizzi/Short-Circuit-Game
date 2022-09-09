using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ChargeEnemyLogic : EnemyLogic {
    [SerializeField] GameObject Teeth;
    [SerializeField] float chargeCooldownTime = 2f;
    [SerializeField] float trackingTime = 2f;
    [SerializeField] float chargeForce = 1.0f;
    [SerializeField] float giveUpTime = 10.0f;

    

    private float lastChargeTime = -100f;
    private bool currentlyCharging = false;
    private Vector3 targetPosition = Vector3.zero;
    private bool inVision = false;
    private EnemyActions currentAction = EnemyActions.Patrolling;
    private float aggroTime = 1.0f;
    private Animator animator;
    private ParticleSystem particles;
    private float particleDuration;

    Vector3 goingToPoint;
    float lastGoTime;
    float goLength;

    protected override void Start() {
        base.Start();
        animator = GetComponent<Animator>();
        particles = GetComponentInChildren<ParticleSystem>();
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
                // Attacking
                if (!currentlyCharging) {
                    SmoothLookAt(targetPosition);
                }

                float timeSinceLastCharge = Time.time - lastChargeTime;
                if (timeSinceLastCharge > chargeCooldownTime && !currentlyCharging) { 
                    lastChargeTime = Time.time;
                    StartCoroutine(ChargeCoroutine());
                }
                break;

            case EnemyActions.Pursuing:
                break;

            case EnemyActions.Searching:
                break;
        }
    }


    IEnumerator ChargeCoroutine() {
        currentlyCharging = true;

        // Charge after locking onto enemy for enough time
        animator?.SetTrigger("Aggro");
        float trackedTime = 0;
        while (trackedTime < trackingTime) {
            float angle = Vector2.Angle(this.transform.up, targetPosition - this.transform.position);
            if (angle < 20f) {
                trackedTime += Time.deltaTime;
            }
            SmoothLookAt(targetPosition);
            yield return null;
        }

        // Charge!
        animator?.SetTrigger("Charge");
        particles?.Play();
        Teeth.SetActive(true);
        AudioManager.PlayClipAtPoint(transform.position, "Enemy Charge");

        rb.freezeRotation = true;
        int mask = LayerMask.GetMask("Wall", "Player");
        float chargeStartTime = Time.time;
        rb.velocity = transform.up.normalized * chargeForce;
        RaycastHit2D hit = Physics2D.CircleCast(this.transform.position, 0.5f, this.transform.up, 1.2f, mask);
        while(!hit && Time.time - chargeStartTime < giveUpTime) {
            // Debug.DrawLine(this.transform.position, this.transform.position + this.transform.up, Color.red, 1f);
            rb.velocity = transform.up.normalized * chargeForce;
            yield return null;
            hit = Physics2D.CircleCast(this.transform.position, 0.5f, this.transform.up, 1.2f, mask);
        }

        if (Time.time - chargeStartTime < giveUpTime && LayerMask.LayerToName(hit.collider.gameObject.layer) == "Wall") {
            // we hit something probs
            AudioManager.PlayClipAtPoint(this.transform.position, "ChargerHit");
        }

        // Slow down
        rb.velocity *= 0.5f;
        particles?.Stop();

        yield return new WaitForSeconds(0.5f);
        Teeth.SetActive(false);
        animator?.SetTrigger("Relax");

        rb.freezeRotation = false;

        currentlyCharging = false;
    }

    protected override void OnVisionEnter(Transform t) {
        base.OnVisionEnter(t);
        targetPosition = t.position;
        currentAction = EnemyActions.Attacking;
    }

    protected override void OnVisionStay(Transform t) {
        base.OnVisionStay(t);
        targetPosition = t.position;
    }

    protected override void OnVisionExit(Transform t) {
        base.OnVisionExit(t);
        MoveTowards(t.position);
        currentAction = EnemyActions.Pursuing;
    }
}
