using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionOrdering : MonoBehaviour
{
    [SerializeField] OrderedCollision[] collisionOrder;

    // The sourceObject is the source of this gameobject has its collisions ignored
    // Null sourceObjects are ignored
    public GameObject sourceObject = null;

    private List<OrderedCollision> GetComponentsInOrder()
    {
       var allComponents = new List<OrderedCollision>(GetComponents<OrderedCollision>());
        List<OrderedCollision> orderedComponents = new List<OrderedCollision>();
        foreach(OrderedCollision oc in collisionOrder)
        {
            if (allComponents.Contains(oc))
            {
                orderedComponents.Add(oc);
                allComponents.Remove(oc);
            }
        }
        orderedComponents.AddRange(allComponents);
        return orderedComponents;
    }

    // Detect Collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (checkIfCollisionSource(collision.collider.gameObject))
            return;

        GetComponentsInOrder().ForEach(x => x.OrderedOnCollisionEnter(collision));
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (checkIfCollisionSource(collision.collider.gameObject))
            return;

        GetComponentsInOrder().ForEach(x => x.OrderedOnCollisionStay(collision));
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (checkIfCollisionSource(collision.collider.gameObject))
            return;

        GetComponentsInOrder().ForEach(x => x.OrderedOnCollisionExit(collision));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (checkIfCollisionSource(other.gameObject))
            return;

        GetComponentsInOrder().ForEach(x => x.OrderedOnTriggerEnter(other));
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (checkIfCollisionSource(other.gameObject))
            return;

        GetComponentsInOrder().ForEach(x => x.OrderedOnTriggerStay(other));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (checkIfCollisionSource(other.gameObject))
            return;

        GetComponentsInOrder().ForEach(x => x.OrderedOnTriggerExit(other));
    }

    bool checkIfCollisionSource(GameObject source)
    {
        if (sourceObject == null)
            return false;

        return source == sourceObject;
    }
}
