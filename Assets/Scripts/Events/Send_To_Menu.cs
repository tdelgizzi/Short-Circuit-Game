using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Send_To_Menu : MonoBehaviour
{


    [SerializeField]
    private float duration_wait_sec;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TESTING1");
        if (collision.tag == "Player")
        {
            Debug.Log("TESTING2");
            StartCoroutine(sendToMenu());
        }



    }

    IEnumerator sendToMenu()
    {
        Debug.Log("TESTING");
        yield return new WaitForSeconds(duration_wait_sec);
        SceneTransitioner.Instance.LoadScene("MenuScreen");
    }
}
