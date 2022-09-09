using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    [SerializeField] KeyCode key;
    [SerializeField] float distance;
    void Update()
    {
        if(Input.GetKeyDown(key)) {
            Vector3 dashPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dashPoint.z = this.transform.position.z;
            dashPoint = dashPoint - this.transform.position;
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, dashPoint, dashPoint.magnitude);
            if (hit.collider != null)
            {
                print(hit.collider.name);
                this.transform.position = (dashPoint.normalized * hit.distance) + this.transform.position;
            }
      
            if (dashPoint.magnitude > distance)
            {
                this.transform.position = (dashPoint.normalized * distance) + this.transform.position;
            }
            else
            {
                this.transform.position = (dashPoint) + this.transform.position;
            }
        }
    }
}
