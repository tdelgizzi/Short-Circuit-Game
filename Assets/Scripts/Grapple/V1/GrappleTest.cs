using UnityEngine;

public class GrappleTest : MonoBehaviour
{
    [SerializeField] GrappleManager Manager;
    [SerializeField] KeyCode Key;

    void Update()
    {
        if (PauseManager.Instance.IsPaused()) return;
        if (Input.GetKeyDown(Key)) Manager?.StartGrapple();
        if (Input.GetKeyUp(Key)) Manager?.EndGrapple();
    }
}
