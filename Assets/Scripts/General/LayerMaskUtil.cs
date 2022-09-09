using UnityEngine;

public class LayerMaskUtil {
    public static bool IsInLayerMask(int layer, LayerMask mask) {
        return (mask.value == (mask.value | (1 << layer)));
    }
}