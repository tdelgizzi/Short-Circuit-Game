using UnityEngine;
using UnityEngine.SceneManagement;

public class AnalyticsConsent : MonoBehaviour
{
    [SerializeField] string NextScene = "GameScene";

    public void Yes()
    {
        AnalyticsManager.IsAnalyticsEnabled = true;
        SceneTransitioner.Instance.LoadScene(NextScene);
        // SceneManager.LoadScene(NextScene);
    }

    public void No()
    {
        AnalyticsManager.IsAnalyticsEnabled = false;
        // SceneManager.LoadScene(NextScene);
        SceneTransitioner.Instance.LoadScene(NextScene);
    }
}
