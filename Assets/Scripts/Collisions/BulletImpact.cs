using UnityEngine;

public class BulletImpact : OrderedCollision
{
    [SerializeField] LayerMask ignoreLayers;
    [SerializeField] GameObject bulletImpactPrefab;

    public override void OrderedOnCollisionEnter(Collision2D other)
    {
        
        if (!LayerMaskUtil.IsInLayerMask(other.gameObject.layer, ignoreLayers))
        {
            Instantiate(bulletImpactPrefab, transform.position, transform.rotation);
        }
    }

    public override void OrderedOnTriggerEnter(Collider2D other)
    {
        if (!LayerMaskUtil.IsInLayerMask(other.gameObject.layer, ignoreLayers))
        {
            Instantiate(bulletImpactPrefab, transform.position, transform.rotation);
        }
    }

}
