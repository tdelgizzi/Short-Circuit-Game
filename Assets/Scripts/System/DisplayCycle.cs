using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCycle : MonoBehaviour
{
    private Text cycleText;

    private void Start()
    {
        cycleText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCycle();
    }

    void UpdateCycle()
    {
        cycleText.text = Cycles.Instance.GetCycle().ToString();
    }
}
