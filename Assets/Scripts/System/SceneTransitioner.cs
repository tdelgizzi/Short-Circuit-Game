using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour {

    public static SceneTransitioner Instance;

    [SerializeField] GameObject blackScreenPrefab;

    List<string> requestQueue = new List<string>();
    IEnumerator currentTransition;
    CameraGlitch glitch;


    void Awake() {
        if (Instance && Instance != this) {
            Destroy(this.gameObject);
        }
        else {
            Instance = this;   
        }
        currentTransition = null;
    }

    void Start() {
        glitch = Camera.main.GetComponent<CameraGlitch>();
    }

    public void LoadScene(string sceneName) {
        if (currentTransition != null) {
            requestQueue.Add(sceneName);
        } else {
            currentTransition = TransitionCoroutine(sceneName);
            StartCoroutine(currentTransition);
        }
    }

    IEnumerator TransitionCoroutine(string sceneName) {
        
        // Slight delay before starting transition
        yield return new WaitForSeconds(0.5f);

        // Glitch out
        // yield return SetGlitch(0.003f, 0.01f, 0.15f);
        // yield return SetGlitch(-0.003f, -0.01f, 0.15f);
        // yield return SetGlitch(0, 0, 0.1f);

        // yield return SetGlitch(0.003f, 0.01f, 0.1f);
        // yield return SetGlitch(-0.003f, -0.01f, 0.1f);
        // yield return SetGlitch(0, 0, 0.05f);

        // yield return SetGlitch(0.006f, 1f, 0.15f);
        // yield return SetGlitch(-0.006f, -1f, 0.15f);

        // Black out this scene
        GameObject blackScreen = BlackOut();
        CanvasGroup blackScreenUI = blackScreen.GetComponent<CanvasGroup>();

        float startTime = Time.time;
        float progress = 0;
        float startAlpha = blackScreenUI.alpha;
        while (progress < 1.0f) {
            progress = (Time.time - startTime) / 1.5f;
            blackScreenUI.alpha = Mathf.Lerp(startAlpha, 1.0f, progress);
            yield return null;
        }

        AudioManager.PlayClipNow("MenuStatic");

        yield return new WaitForSeconds(2.0f);

        // Make lines more visible
        // glitch.SetLineFactorAndSpeed(0.1f, 0.1f);

        // yield return new WaitForSeconds(0.1f);

        // // Load next scene
        // yield return SetGlitch(0, 0, 0);
        SceneManager.LoadScene(sceneName);
        // yield return SetGlitch(-0.006f, -1f, 0.15f);

        // Black out next scene once loaded
        while (SceneManager.GetActiveScene().name != sceneName) {
            yield return null;
        }
        blackScreen = BlackOut();
        blackScreenUI = blackScreen.GetComponent<CanvasGroup>();
        blackScreenUI.alpha = 1;

        startTime = Time.time;
        progress = 0;
        startAlpha = blackScreenUI.alpha;
        while (progress < 1.0f) {
            progress = (Time.time - startTime) / 1.5f;
            blackScreenUI.alpha = Mathf.Lerp(startAlpha, 0f, progress);
            yield return null;
        }

        // Remove black out after delay
        yield return new WaitForSeconds(0.5f);
        blackScreen.SetActive(false);
        // glitch.SetLineFactorAndSpeed(0.04f, 0.05f);

        // Glitch back
        // yield return SetGlitch(0.003f, 0.01f, 0.15f);
        // yield return SetGlitch(-0.003f, -0.01f, 0.15f);
        // yield return SetGlitch(0, 0, 0.1f);

        // yield return SetGlitch(0.003f, 0.01f, 0.1f);
        // yield return SetGlitch(-0.003f, -0.01f, 0.1f);
        // yield return SetGlitch(0, 0, 0.05f);

        // start next transition if there is one
        currentTransition = null;
        if (requestQueue.Count > 0) {
            string nextTransition = requestQueue[0];
            requestQueue.RemoveAt(0);
            LoadScene(nextTransition);
        }
    }

    IEnumerator SetGlitch(float splitFactor, float staticFactor, float time) {
        glitch.SetSplitFactor(splitFactor);
        glitch.SetStaticFactor(staticFactor);
        yield return new WaitForSeconds(time);
    }

    GameObject BlackOut() {
        GameObject screen = GameObject.Instantiate(blackScreenPrefab);
        screen.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        return screen;
    }
}
