using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutInput : MonoBehaviour
{
    [SerializeField] GameObject LoadoutScreen;
    [SerializeField] KeyCode LoadoutHotkey;
    [SerializeField] GameObject WeaponsRoom;

    void Update()
    {
        // We use a 3d collider to prevent interaction with bullets
        var box = WeaponsRoom.GetComponent<BoxCollider>();
        var insideWeaponsRoom = box.bounds.Contains(transform.position);

        if (Input.GetKeyDown(LoadoutHotkey) && insideWeaponsRoom && !PauseManager.Instance.IsPaused()) 
        {
            LoadoutScreen.SetActive(true);
            PauseManager.Instance.SetPauseState(true);
        }
    }
}
