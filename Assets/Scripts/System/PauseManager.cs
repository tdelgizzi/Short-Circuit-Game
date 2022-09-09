using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {
   
    [SerializeField] bool isPaused;
    public static PauseManager Instance;

    void Awake() {
        if (Instance && Instance != this) {
            Destroy(this.gameObject);
        }
        else {
            Instance = this;   
        }
    }

    public void SetPauseState(bool pause) {
        isPaused = pause;
        Time.timeScale = isPaused ? 0 : 1;
    }

    public bool IsPaused() {
        return isPaused;
    }
}
