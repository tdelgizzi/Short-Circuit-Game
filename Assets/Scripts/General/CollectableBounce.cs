using UnityEngine;

public class CollectableBounce : MonoBehaviour
{
    [SerializeField] float amplitude;
    [SerializeField] float frequency;

    void Start()
    {
        
    }

    void Update()
    {
        var newPosition = transform.position;
        newPosition.y += Mathf.Sin(Time.time * frequency) * amplitude * Time.deltaTime;
        transform.position = newPosition;
    }
}
