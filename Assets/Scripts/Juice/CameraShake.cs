using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float LerpConstant = 0.5f;
    [SerializeField] float GlobalDampeningFactor = 0.1f;

    public void ShakeOnce(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude * GlobalDampeningFactor));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        float time = 0.0f;
        while (time < duration)
        {
            var offset = Random.onUnitSphere * magnitude;
            offset.z = 0;

            transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition + offset, LerpConstant);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
