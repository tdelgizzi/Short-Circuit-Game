using UnityEngine;

public class ChangeLayerOnReflection : MonoBehaviour
{
    [SerializeField] string layerName;

    public void ChangeToLayer()
    {
        int layer = LayerMask.NameToLayer(layerName);
        gameObject.layer = layer;
    }
}
