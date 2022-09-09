using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltersHealth : OrderedCollision {
    [SerializeField] int amount;

    public override void OrderedOnCollisionEnter(Collision2D other)
    {
        HasHealth otherHealth = other.collider.gameObject.GetComponent<HasHealth>();
        if (otherHealth)
        {
            otherHealth.UpdateHealth(amount);
        }
    }

     public override void OrderedOnTriggerEnter(Collider2D other) {
        HasHealth otherHealth = other.gameObject.GetComponent<HasHealth>();
        if (otherHealth) {
            otherHealth.UpdateHealth(amount);
        }
    }

    public void SetAmount(int _amount) {
        amount = _amount;
    }
}
