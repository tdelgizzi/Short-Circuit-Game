using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Randomly moves around a point
public class FollowMouse : MonoBehaviour {

    [SerializeField] float speed;
    [SerializeField] AnimationCurve curve;

    Camera mainCamera;


    void Start() {
        mainCamera = Camera.main;
    }

    void FixedUpdate() {
        Vector3 mousPos = mainCamera.ScreenToWorldPoint(Input.mousePosition); 

        Vector3 lerpedPos = Vector3.Lerp(this.transform.position, mousPos, speed * curve.Evaluate(Time.deltaTime));
        this.transform.position = lerpedPos;
    }
}
