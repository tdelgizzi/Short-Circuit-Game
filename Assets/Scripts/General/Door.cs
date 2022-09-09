using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    [SerializeField] Vector3 closePosition;
    [SerializeField] float closeTime;

    public void Close() {
        StartCoroutine(CloseRoutine());
    }

    IEnumerator CloseRoutine() {
        Vector3 initPos = this.transform.position;
        float startTime = Time.time;
        float progress = 0;
        AudioManager.PlayClipAtPoint(this.transform.position, "DoorClosing");
        while (progress < 1.0f) {
            progress = (Time.time - startTime) / closeTime;
            this.transform.position = Vector3.Lerp(initPos, closePosition, progress);
            yield return null;
        }
    }
}
