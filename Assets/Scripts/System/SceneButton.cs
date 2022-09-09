using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneButton : MonoBehaviour {

    [SerializeField] UnityEvent onClick;
    [SerializeField] Camera mainCamera;
    [SerializeField] TextMeshPro buttonText;
    [SerializeField] Color activeColor;

    Vector3 mousePos;
    Vector3[] corners = new Vector3[4];
    bool isActive = false;

    void Start() {
        RectTransform transform = this.GetComponent<RectTransform>();
        transform.GetWorldCorners(corners);
    }

    void Update() {
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        bool hovering = IsWithinCorners(mousePos);

        if (hovering && !isActive) {
            // Show background color
            isActive = true; 
        }
        else if (!hovering && isActive) {
            // Hide background color
            isActive = false;
        }
    }

    bool IsWithinCorners(Vector3 pos) {
        return pos.x >= corners[0].x && pos.x >= corners[3].x
                && pos.x <= corners[1].x && pos.x <= corners[2].x
                && pos.y >= corners[3].y && pos.y >= corners[2].y
                && pos.y <= corners[0].y && pos.y <= corners[1].y;
    }
}
