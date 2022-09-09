using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStraight : MonoBehaviour {

    [SerializeField] float speed;   
    
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void Update() {
        rb.velocity = this.transform.up * speed;
    }

    public void SetSpeed(float s) {
        speed = s;
    }
}
