using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastSender : MonoBehaviour
{

    [SerializeField]
    private string message;
    [SerializeField]
    private float duration = 3f;
    // Start is called before the first frame update
    [SerializeField]
    private bool keyPressToast = false;
    [SerializeField]
    private KeyCode keyPressKey;
    [SerializeField]
    private string aclip;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision w/ ToastSender Block");
        if (other.tag == "Player" )
        {
            if (keyPressToast)
            {
                Debug.Log("Player hit toast trigger block w keyPress");
                EventBus.Publish<ToastEvent>(new ToastEvent(message, keyPressKey, aclip));
                GameObject me = this.transform.gameObject;
                me.SetActive(false);
            }
            else
            {
                Debug.Log("Player hit toast trigger block");
                EventBus.Publish<ToastEvent>(new ToastEvent(message, duration, aclip));
                GameObject me = this.transform.gameObject;
                BoxCollider2D bx = me.GetComponent<BoxCollider2D>();
                //me.SetActive(false);
                bx.enabled = false;
            }
        }
    }
}
