using UnityEngine;

public class Menu : MonoBehaviour {

    public void LoadGame() {
        SceneTransitioner.Instance.LoadScene("Ship");
        PauseManager.Instance.SetPauseState(false);
        GameManager.Instance.ResetGame();
    }

    public void LoadGameWithCutScene()
    {
        SceneTransitioner.Instance.LoadScene("CutScene");
    }

    public void LoadMainMenu() {
        SceneTransitioner.Instance.LoadScene("MenuScreen");
        // Menu should not be paused
        PauseManager.Instance.SetPauseState(false);
    }

    public void LoadTutorial()
    {
        SceneTransitioner.Instance.LoadScene("Tutorial");
        PauseManager.Instance.SetPauseState(false);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
