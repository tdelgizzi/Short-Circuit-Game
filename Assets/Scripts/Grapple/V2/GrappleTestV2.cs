using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleTestV2 : MonoBehaviour
{
    [SerializeField] GrappleManagerV2 Manager;
    [SerializeField] KeyCode Key;

    void Update()
    {
        if (PauseManager.Instance.IsPaused()) return;
        if (Input.GetKeyDown(Key)) Manager?.StartGrapple();
        if (Input.GetKeyUp(Key)) Manager?.EndGrapple();
    }
}
