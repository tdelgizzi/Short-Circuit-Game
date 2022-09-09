using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cycles : MonoBehaviour
{
    private int cycle = 1;

    private static Cycles _instance;

    public static Cycles Instance
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

    public void ResetCycle()
    {
        cycle = 1;
    }

    public void IncrementCycle()
    {
        cycle += 1;
    }

    public int GetCycle()
    {
        return cycle;
    }

}
