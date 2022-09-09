using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;
using TMPro;

public class EndScene : MonoBehaviour {

    [SerializeField] List<GameObject> doors;
    [SerializeField] GameObject endButton;
    [SerializeField] Light2D sceneLights;
    [SerializeField] GameObject player;
    [SerializeField] CanvasGroup enterTextUI;
    [SerializeField] CanvasGroup UIGroup;
    [SerializeField] CanvasGroup bars;
    [SerializeField] GameObject scoreScreen;
    [SerializeField] List<GameObject> scoreScreenElements;
    [SerializeField] Transform spawnRoomCameraPosition;
    [SerializeField] Transform endScreenCameraPosition;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI robotText;

    CameraOverheadFollow cameraOverheadFollow;

    public void Initiate() {
        endButton.SetActive(true);
        cameraOverheadFollow = Camera.main.GetComponent<CameraOverheadFollow>();
    }

    public void Conclusion() {
        StartCoroutine(ConcludeEndSceneRoutine());
    }

    IEnumerator ConcludeEndSceneRoutine() {

        endButton.SetActive(false);
        // Make sure UI is gone
        UIGroup.alpha = 0;

        // Fix position of spawn room camera
        cameraOverheadFollow.SetFollowObject(spawnRoomCameraPosition);

        // close doors
        foreach (GameObject door in doors) {
            Door d = door.GetComponent<Door>();
            d.Close();
        }

        // Fade out enter UI
        float startTime = Time.time;
        float progress = 0;
        float startAlpha = enterTextUI.alpha;
        while (progress < 1.0f) {
            progress = (Time.time - startTime) / 0.5f;
            enterTextUI.alpha = Mathf.Lerp(startAlpha, 0, progress);
            yield return null;
        }

        // Bring up lights
        startTime = Time.time;
        progress = 0;
        float startIntensity = sceneLights.intensity;
        while (progress < 1.0f) {
            progress = (Time.time - startTime) / 2.0f;
            sceneLights.intensity = Mathf.Lerp(startIntensity, 1.0f, progress);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        AudioManager.PlayClipNow("CoreEngaging");

        // FADE OUT MUSIC
        startTime = Time.time;
        progress = 0;
        AudioSource backgroundMusic = AudioManager._instance.GetBackgroundMusic();
        float startVolume = backgroundMusic.volume;
        startAlpha = bars.alpha;
        while (progress < 1.0f) {
            progress = (Time.time - startTime) / 2.0f;
            backgroundMusic.volume = Mathf.Lerp(startVolume, 0f, progress);
            yield return null;
        }

        
        yield return new WaitForSeconds(1f);
        AudioManager.PlayClipNow("PoweringUp");


        // FADE UP LIGHTS
        startTime = Time.time;
        progress = 0;
        startIntensity = sceneLights.intensity;
        startAlpha = bars.alpha;
        while (progress < 1.0f) {
            progress = (Time.time - startTime) / 4f;
            sceneLights.intensity = Mathf.Lerp(startIntensity, 5.0f, progress);
            bars.alpha = Mathf.Lerp(startAlpha, 0f, progress);
            yield return null;
        }

        // Warp lights
        startTime = Time.time;
        progress = 0;
        startIntensity = sceneLights.intensity;
        while (progress < 1.0f) {
            progress = (Time.time - startTime) / 2.0f;
            sceneLights.intensity = Mathf.Lerp(startIntensity, 40.0f, progress);
            yield return null;
        }
        
        AudioManager.PlayClipNow("WarpDrive");

        // Warp camera to space
        cameraOverheadFollow.SetFollowSpeed(1000);
        cameraOverheadFollow.SetFollowObject(endScreenCameraPosition);

        // Drop scene lights intensity 
        startTime = Time.time;
        progress = 0;
        startIntensity = sceneLights.intensity;
        while (progress < 1.0f) {
            progress = (Time.time - startTime) / 0.5f;
            sceneLights.intensity = Mathf.Lerp(startIntensity, 1.0f, progress);
            yield return null;
        }

        // TODO: Spawn in text on end screen sequentially
        TimeSpan timeSpan = TimeSpan.FromSeconds(Time.time - GameManager.Instance.GetStartTime());
        timeText.text = $"{(int)timeSpan.TotalMinutes}:{timeSpan.Seconds:00}";
        robotText.text = (GameManager.Instance.GetRobots() + 1).ToString();

        scoreScreen.SetActive(true);

        yield return new WaitForSeconds(3.0f);


        foreach (GameObject c in scoreScreenElements) {
            c.SetActive(true);
            yield return new WaitForSeconds(1.2f);
        }

        // So player can't move around
        PauseManager.Instance.SetPauseState(true);
        // FIN~
    }
}
