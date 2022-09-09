using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstMovement : MonoBehaviour
{
    public float burstForce = 10;
    Vector3 burstVector;
    Rigidbody2D rb;
    float inputX;
    float inputY;
    [SerializeField] KeyCode key;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!PauseManager.Instance.IsPaused())
        {
            //inputX = Input.GetAxisRaw("Vertical");
            //inputY = Input.GetAxisRaw("Horizontal");

            burstVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(rb.position.x, rb.position.y);
            burstVector = burstVector.normalized * burstForce;

            if (Input.GetKeyDown(key))
            {
                rb.AddForce(burstVector);
            }
        }
    }

    void Burst()
    {
        rb.AddForce(Vector3.up * inputX * burstForce);
        rb.AddForce(Vector3.right * inputY * burstForce);
    }
}
