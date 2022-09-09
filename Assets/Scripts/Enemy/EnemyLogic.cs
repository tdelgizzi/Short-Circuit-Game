using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour {

    protected enum EnemyActions {
        Patrolling, // Travelling between waypoints or just standing still if there are no waypoints
        Attacking, // Moving towards the player, swinging
        Pursuing, // Player has gone out of view, we cannot attack, just move towards the last known position
        Searching // Look around to gain vision of the player
    }

    protected FieldOfView fov;
    protected Rigidbody2D rb;

    public float lookSpeed = 1;
    public List<Transform> lastVisibleTargets = new List<Transform>();
    public float movementSpeed = 1.0f;

    private bool isMoving = false;
    protected Vector2 destination;
    private bool isAggressive = false;

    protected virtual void Start() {
        fov = GetComponent<FieldOfView>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update() {
        List<Transform> curVisibleTargets = new List<Transform>(fov.visibleTargets);

        foreach (Transform t in curVisibleTargets) {
            // Target was in vision last frame
            if (lastVisibleTargets.Contains(t)) {
                lastVisibleTargets.Remove(t);
            }
            // New target enters vision
            else {
                OnVisionEnter(t);
            }

            OnVisionStay(t);
        }

        foreach (Transform t in lastVisibleTargets) {
            OnVisionExit(t);
        }

        lastVisibleTargets.Clear();
        lastVisibleTargets = new List<Transform>(fov.visibleTargets);
    }

    void FixedUpdate() {
        if (isMoving) {
            Vector2 direction = ((Vector3) destination - transform.position).normalized;
            rb.AddForce(direction * movementSpeed);

            if (IsCloseEnoughToPosition(destination)) {
                isMoving = false;
            }
        }
    }

    // Called when target enters vision
    protected virtual void OnVisionEnter(Transform t) {
    }

    // Called every frame target is in vision
    protected virtual void OnVisionStay(Transform t) {
    }

    // Called the frame AFTER target leaves vision
    protected virtual void OnVisionExit(Transform t) {
    }


    protected void SmoothLookAt(Transform lookObject) {
        SmoothLookAt(lookObject.position);
    }

    protected void SmoothLookAt(Vector3 lookPosition) {
        Quaternion targetRotation = Quaternion.Euler(0, 0, GetAngle(lookPosition));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);
    }

    protected float GetAngle(Transform lookObject) {
        return GetAngle(lookObject.position);
    }

    protected float GetAngle(Vector3 lookPosition) {
        Vector2 lookDirection = lookPosition - transform.position;
        float lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        return lookAngle;
    }

    protected float GetRelativeAngle(Transform lookObject) {
        return GetRelativeAngle(lookObject.position);
    }

    protected float GetRelativeAngle(Vector3 lookPosition) {
        Vector3 dir = lookPosition - transform.position;
        dir = transform.InverseTransformDirection(dir);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        return angle;
    }

    protected void MoveTowards(Vector3 position) {
        var waypoints = NavMeshFacade.Instance.GetPath(transform.position, position);
        if (waypoints.Count > 1) position = waypoints[1];

        SmoothLookAt(position);
        destination = position;
        isMoving = true;
    }

    protected void MoveDirectlyTowards(Vector3 position) {
        SmoothLookAt(position);
        
        destination = position;
        isMoving = true;
    }

    protected bool IsCloseEnoughToPosition(Vector3 position) {
        var distance = ((Vector2)transform.position - (Vector2)position).magnitude;
        return distance < 1.0f;
    }

    // Return the first visible target transform in field of view.
    Transform GetFirstTargetVisible() {
        Transform target = null;

        foreach (Transform t in fov.visibleTargets) {
            if (t.gameObject.activeSelf)
                target = t;
        }

        return target;
    }

    public void SetAggressive(bool on) {
        isAggressive = on;
    }
}
