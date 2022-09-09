using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float ParallaxFactor;

    private Vector3 startPosition;
    private Vector3 cameraStartPosition;


    void Start()
    {
        startPosition = transform.position;
        cameraStartPosition = Camera.main.transform.position;
    }

    void Update()
    {
        var cameraOffset = Camera.main.transform.position - cameraStartPosition;
        transform.position = new Vector3(startPosition.x + cameraOffset.x * ParallaxFactor,
                                         startPosition.y + cameraOffset.y * ParallaxFactor,
                                         startPosition.z);
    }
}
