using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    [SerializeField] int goal;
    [SerializeField] GameObject winScreen;

    void Update()
    {
        if (Score.Instance.GetScore() >= goal)
        {
            PauseManager.Instance.SetPauseState(true);
            winScreen.SetActive(true);

        }
    }
}
