using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public Texture2D reticle;

    private void Start()
    {
        Cursor.SetCursor(reticle, new Vector2(16, 16), CursorMode.ForceSoftware);
        //Cursor.lockState = CursorLockMode.Confined;
    }
}
