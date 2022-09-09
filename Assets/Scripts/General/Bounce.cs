using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour {

    [SerializeField] float bounceFrequency;
    [SerializeField] float upwardsBounceDistance;

    void Start() {
        StartCoroutine(BounceCoroutine());
    }

    IEnumerator BounceCoroutine() {
        while(true) {
            Vector3 initPos = this.transform.position;
            Vector3 finalPos = initPos + this.transform.up * upwardsBounceDistance; 
            float startTime = Time.time;
            float progress = 0;
            while (progress < 1.0f) {
                progress = (Time.time - startTime) / bounceFrequency;
                this.transform.position = Vector3.Lerp(initPos, finalPos, progress);
                yield return null;
            }

            initPos = this.transform.position;
            finalPos = initPos - this.transform.up * upwardsBounceDistance; 
            startTime = Time.time;
            progress = 0;
            while (progress < 1.0f) {
                progress = (Time.time - startTime) / bounceFrequency;
                this.transform.position = Vector3.Lerp(initPos, finalPos, progress);
                yield return null;
            }
        }
    }
}
