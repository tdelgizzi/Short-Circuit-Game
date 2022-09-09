using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerDirection : MonoBehaviour {
    public bool controlEnabled = true;

    Rigidbody2D rb;
    Camera mainCamera;
    Vector2 mousePos;
    Vector3 lookDirection;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    void Update() {
        if (controlEnabled && !PauseManager.Instance.IsPaused()) {
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    void FixedUpdate() {
        if (!controlEnabled || PauseManager.Instance.IsPaused())
            return;

        lookDirection = mousePos - new Vector2(rb.position.x, rb.position.y);
        rb.rotation = GetCurrentRotation();
    }

    public Vector3 GetMousePosition() {
        return mousePos;
    }

    public Vector3 GetCurrentDirection() {
        return lookDirection;
    }

    public float GetCurrentRotation() {
        return Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
    }
}
