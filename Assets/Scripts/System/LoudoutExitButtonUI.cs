using UnityEngine;
using UnityEngine.UI;

public class LoudoutExitButtonUI : MonoBehaviour
{
    [SerializeField] LoadoutUI loadout;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(loadout.ExitLoadout);
    }
}
