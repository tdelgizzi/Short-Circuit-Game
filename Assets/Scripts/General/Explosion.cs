using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float explosionForce;
    [SerializeField] float explosionRadius;
    [SerializeField] float fuseTime;
    [SerializeField] KeyCode key;
    [SerializeField] GameObject explosion;
    void Awake()
    {
        StartCoroutine(StartFuse(fuseTime));
    }

    private void Update()
    {
        if(Input.GetKeyDown(key))
        {
            StartCoroutine(Explode());
        }
    }

    IEnumerator Explode()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            //print(hitCollider.name);
            Rigidbody2D rb = hitCollider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 force = rb.transform.position - this.transform.position;
                rb.AddForce(force.normalized * explosionForce);
            }
        }
        AudioManager.PlayClipNow("Explosion");
        explosion.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    IEnumerator StartFuse(float fuseTime)
    {
        yield return new WaitForSeconds(fuseTime);
        StartCoroutine(Explode());
    }
}
