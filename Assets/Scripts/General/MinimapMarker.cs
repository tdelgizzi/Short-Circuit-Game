using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MinimapMarker : MonoBehaviour {
    [SerializeField] bool useParentSprite = true;
    [SerializeField] Sprite markerSprite;
    [SerializeField] float markerSize;

    SpriteRenderer spriteRenderer;

    void Start() {
        spriteRenderer = this.GetComponent<SpriteRenderer>();

        if (useParentSprite) {
            SpriteRenderer parentRenderer = this.transform.parent.gameObject.GetComponentInParent<SpriteRenderer>();
            if (parentRenderer == null) {
                Debug.Log("Error: Marker parent has no sprite renderer, cannot use its sprite for marker");
                return;
            } 
            Debug.Log(parentRenderer.name);
            markerSprite = parentRenderer.sprite;
        }

        spriteRenderer.sprite = markerSprite;
        this.transform.localScale = new Vector3(markerSize, markerSize, 0);
        this.gameObject.layer = LayerMask.NameToLayer("MinimapMarker");
    }
}
