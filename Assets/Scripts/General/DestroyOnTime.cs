using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTime : MonoBehaviour
{
    public float destroyTime;

    void Start()
    {
        Destroy(this.gameObject, destroyTime);
    }
}
