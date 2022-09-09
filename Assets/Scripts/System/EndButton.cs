using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndButton : MonoBehaviour {

    [SerializeField] GameObject target;
    [SerializeField] CanvasGroup enterUIGroup;
    [SerializeField] CanvasGroup UIGroup;
    [SerializeField] EndScene endScene;
    [SerializeField] float distance;

    bool isActive = false;
    bool pressed = false;
    List<SpriteRenderer> sprites = new List<SpriteRenderer>();

    void Start() {
        sprites.AddRange(this.gameObject.GetComponentsInChildren<SpriteRenderer>());
    }

    void Update() {
        if ((target.transform.position - this.transform.position).magnitude < distance) {
            if (!isActive) {
                isActive = true;
                
                // Hide pointer and UI
                foreach (SpriteRenderer s in sprites) {
                    s.enabled = false;
                }
                StartCoroutine(UIAlphaCoroutine(UIGroup, 0f));

                // Enable UI element to press enter
                StartCoroutine(UIAlphaCoroutine(enterUIGroup, 1f));
            } 

            // Check for key press
            if (Input.GetKeyDown(KeyCode.Return) && !pressed) {
                pressed = true;
                endScene.Conclusion();
            }
        }
        else {
            if (isActive) {
                isActive = false;
                
                // Show pointer and UI
                foreach (SpriteRenderer s in sprites) {
                    s.enabled = true;
                }
                StartCoroutine(UIAlphaCoroutine(UIGroup, 1f));

                // Disable UI element to press enter
                StartCoroutine(UIAlphaCoroutine(enterUIGroup, 0f));
            }
        }
    }

    IEnumerator UIAlphaCoroutine(CanvasGroup ui, float endAlpha) {
        float startTime = Time.time;
        float progress = 0;
        float startAlpha = ui.alpha;
        while (progress < 1.0f) {
            progress = (Time.time - startTime) / 0.5f;
            ui.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
            yield return null;
        }
    }
}
