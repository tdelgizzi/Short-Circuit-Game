using UnityEngine;

public class WeaponsRoomPointer : MonoBehaviour
{
    [SerializeField] GameObject weaponRoom;
    [SerializeField] float edgePadding = 8;
    private GameObject pointer;

    void Start()
    {
        pointer = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        var objectiveViewportPoint = Camera.main.WorldToViewportPoint(weaponRoom.transform.position);
        if (ObjectivePointers.IsPointInCamera(objectiveViewportPoint))
        {
            // It is inside the screen, do not show the pointer
            pointer.SetActive(false);
        }
        else
        {
            // It is outside the screen, show the pointer
            ObjectivePointers.SetPointerLocation(pointer, weaponRoom, edgePadding);
            pointer.SetActive(true);
        }
    }
}
