using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalCycle : MonoBehaviour
{
    TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        textMesh.SetText("You saved the ship on cycle " + Cycles.Instance.GetCycle());
    }
}
