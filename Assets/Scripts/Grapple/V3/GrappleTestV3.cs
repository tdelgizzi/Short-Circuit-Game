using UnityEngine;

public class GrappleTestV3 : MonoBehaviour
{
    [SerializeField] GrappleManagerV3 Manager;
    [SerializeField] KeyCode Key;

    void Update()
    {
        if (PauseManager.Instance.IsPaused()) return;
        if (Input.GetKeyDown(Key)) Manager?.StartGrapple();
        if (Input.GetKeyUp(Key)) Manager?.EndGrapple();
    }
}
