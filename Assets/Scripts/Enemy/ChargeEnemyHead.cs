using UnityEngine;

public class ChargeEnemyHead : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] LayerMask LayersThatTriggerShine;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var layer = collision.collider.gameObject.layer;
        if (LayersThatTriggerShine == (LayersThatTriggerShine | (1 << layer)))
        {
            Shine();
        }
    }

    private void Shine()
    {
        if (animator)
        {
            animator.SetTrigger("Shine");
            AudioManager.PlayClipAtPoint(this.transform.position, "ChargerBlock");
        }
    }
}
