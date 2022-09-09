using UnityEngine;

public abstract class OrderedCollision : MonoBehaviour
{
    private bool isBeingOrdered = false;

    private void Awake()
    {
        isBeingOrdered = GetComponent<CollisionOrdering>();
    }


    public virtual void OrderedOnCollisionEnter(Collision2D collision)
    {

    }

    public virtual void OrderedOnCollisionStay(Collision2D collision)
    {

    }

    public virtual void OrderedOnCollisionExit(Collision2D collision)
    {

    }

    public virtual void OrderedOnTriggerEnter(Collider2D other)
    {

    }

    public virtual void OrderedOnTriggerStay(Collider2D other)
    {

    }

    public virtual void OrderedOnTriggerExit(Collider2D other)
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isBeingOrdered) OrderedOnCollisionEnter(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isBeingOrdered) OrderedOnCollisionStay(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!isBeingOrdered) OrderedOnCollisionExit(collision);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isBeingOrdered) OrderedOnTriggerEnter(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isBeingOrdered) OrderedOnTriggerStay(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!isBeingOrdered) OrderedOnTriggerExit(other);
    }
}
