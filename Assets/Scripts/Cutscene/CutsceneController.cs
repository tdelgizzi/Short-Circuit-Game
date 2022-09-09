using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] CutSceneCamControl cameraController;
    [SerializeField] CutsceneToastSender toastController;

    private List<KeyCode> skipKeys = new List<KeyCode>()
    {
        KeyCode.Space,
        KeyCode.KeypadEnter,
        KeyCode.Return,
        KeyCode.Mouse0,
        KeyCode.Mouse1,
        KeyCode.Mouse3
    };

    private void Start()
    {
        StartCoroutine(Cutscene());
    }

    private void Update()
    {
        if (skipKeys.Any(key => Input.GetKeyDown(key))) EndCutscene();
    }

    private IEnumerator Cutscene()
    {
        // Wait for transition/load
        yield return new WaitForSeconds(0);

        // First part
        float duration = 17;
        cameraController?.StartFirstSlide(duration);
        toastController?.SendFirstBatch();
        yield return new WaitForSeconds(duration);

        // Second part
        duration = 28;
        cameraController?.StartSecondSlide(duration);
        toastController?.SendSecondBatch();
        yield return new WaitForSeconds(duration);

        // End
        EndCutscene();
    }

    private void EndCutscene()
    { 
        SceneTransitioner.Instance.LoadScene("Ship");
    }
}
