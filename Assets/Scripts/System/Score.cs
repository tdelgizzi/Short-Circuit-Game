using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int score = 0;
    private static Score _instance;

    public static Score Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public int GetScore()
    {
        return score;
    }

    public void IncrementScore()
    {
        score += 1;
    }

    public void ResetScore()
    {
        score = 0;
    }
}
