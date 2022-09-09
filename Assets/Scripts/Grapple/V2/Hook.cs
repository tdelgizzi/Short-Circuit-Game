using UnityEngine;

public class Hook : MonoBehaviour
{
    private Rigidbody2D rb;

    private bool locked = false;
    private Vector3 lockedPosition = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (locked)
        {
            transform.position = lockedPosition;
            rb.freezeRotation = true;
        }
    }

    public Vector3 GetRopeAttachmentPoint()
    {
        return transform.position - transform.up * 0.25f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Test (turn into layers)
        if (collision.gameObject.name.Contains("RopeLink")) return;

        locked = true;
        lockedPosition = transform.position;
    }

    public bool IsLocked()
    {
        return locked;
    }
}
