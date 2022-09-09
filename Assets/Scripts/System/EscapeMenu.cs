using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenu : MonoBehaviour {
    [SerializeField] private GameObject pauseScreen;
   
    bool isMenuOpen = false;
   
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ToggleMenu();
        }

        if (isMenuOpen)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Confined;
    }

    public void ToggleMenu() {
        isMenuOpen = !isMenuOpen;
        PauseManager.Instance.SetPauseState(isMenuOpen);
        pauseScreen.SetActive(isMenuOpen);
    }
}
