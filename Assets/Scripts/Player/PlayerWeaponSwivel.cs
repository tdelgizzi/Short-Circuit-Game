using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSwivel : MonoBehaviour {

    [SerializeField] float maxSwivelAngle;
    PlayerDirection playerDirection;

    void Start() {
        playerDirection = GetComponentInParent(typeof(PlayerDirection)) as PlayerDirection;
    }

    private void OnEnable()
    {
        playerDirection = GetComponentInParent(typeof(PlayerDirection)) as PlayerDirection;
    }

    void FixedUpdate() {
        Vector3 direction = playerDirection.GetMousePosition() - this.transform.position;
        float rotationAngle = Mathf.Clamp(Vector2.Angle(playerDirection.GetCurrentDirection(), direction), -maxSwivelAngle, maxSwivelAngle);
        
        Vector3 angles = this.transform.eulerAngles;
        angles.z = playerDirection.GetCurrentRotation() + rotationAngle;
        this.transform.eulerAngles = angles;
    }
}
