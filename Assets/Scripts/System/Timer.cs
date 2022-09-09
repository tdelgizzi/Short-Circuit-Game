using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public class Timer : MonoBehaviour
{
    [SerializeField] private float startTime;
    [SerializeField] private GameObject loseScreen;
    private float time;
    private Text timeText;
    void Start()
    {
        time = startTime;
        timeText = GetComponent<Text>();
    }

    void Update()
    {
        if (time > 0)
        {
            UpdateTime();
            time -= Time.deltaTime;
        }
        else
        {
            //Time is up!
            Cycles.Instance.IncrementCycle();
            //ResetTime();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //Time.timeScale = 0;
            PauseManager.Instance.SetPauseState(true);
            loseScreen.SetActive(true);
        }
    }

    void UpdateTime()
    {
        float seconds = Mathf.FloorToInt(time % 60);
        float minutes = Mathf.FloorToInt(time / 60);
        timeText.text = minutes.ToString() + ":" + string.Format("{0:00}", seconds);
    }

    void UpdateTime(float time_in)
    {
        float seconds = Mathf.FloorToInt(time_in % 60);
        float minutes = Mathf.FloorToInt(time_in / 60);
        timeText.text = minutes.ToString() + ":" + string.Format("{0:00}", seconds);
    }

    public void ResetTime()
    {
        time = startTime;
    }
}
