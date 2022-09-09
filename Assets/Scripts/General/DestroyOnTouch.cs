using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTouch : OrderedCollision {
    [SerializeField] LayerMask ignoreLayers;

    public override void OrderedOnCollisionEnter(Collision2D other) {
        if (!LayerMaskUtil.IsInLayerMask(other.gameObject.layer, ignoreLayers)) {
            ObjectDestroy();
        }
    }

    public override void OrderedOnTriggerEnter(Collider2D other) {
        if (!LayerMaskUtil.IsInLayerMask(other.gameObject.layer, ignoreLayers))
        {
            ObjectDestroy();
        }
    }

    void ObjectDestroy()
    {
        Destroy(this.gameObject);
    }
}
