using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Randomly moves around a point
public class RandomMovement : MonoBehaviour {

    [SerializeField] float range;
    [SerializeField] float frequency;
    [SerializeField] float speed;
    [SerializeField] AnimationCurve curve;

    Vector3 initialPosition;
    Vector3 finalPosition;
    float lastUpdate = 0;

    void Start() {
        initialPosition = this.transform.position;
        lastUpdate = Time.time - frequency;
    }

    void FixedUpdate() {
        if (Time.time - lastUpdate >= frequency) {
            Vector3 direction = Random.insideUnitCircle * range;
            finalPosition = initialPosition + direction;
            lastUpdate = Time.time;
        }

        Vector3 lerpedPos = Vector3.Lerp(this.transform.position, finalPosition, speed * curve.Evaluate(Time.deltaTime));
        this.transform.position = lerpedPos;
    }
}
