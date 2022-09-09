using UnityEngine;

public class CutSceneCamControl : MonoBehaviour
{
    [SerializeField] Transform firstSlideStart;
    [SerializeField] Transform firstSlideEnd;

    [SerializeField] Transform secondSlideStart;
    [SerializeField] Transform secondSlideEnd;

    private float startTime = 0;
    private float endTime = 0;

    private bool firstSlide = false;
    private bool secondSlide = false;

    void Update()
    {
        if (firstSlide)
        {
            var progression = (Time.time - startTime) / (endTime - startTime);
            transform.position = Vector3.Lerp(firstSlideStart.position, firstSlideEnd.position, progression);

            if (Time.time > endTime) firstSlide = false;
        }
        else if (secondSlide)
        {
            var progression = (Time.time - startTime) / (endTime - startTime);
            transform.position = Vector3.Lerp(secondSlideStart.position, secondSlideEnd.position, progression);

            if (Time.time > endTime) secondSlide = false;
        }
    }

    public void StartFirstSlide(float duration)
    {
        transform.position = firstSlideStart.position;
        firstSlide = true;

        startTime = Time.time;
        endTime = startTime + duration;
    }

    public void StartSecondSlide(float duration)
    {
        transform.position = secondSlideStart.position;
        secondSlide = true;

        startTime = Time.time;
        endTime = startTime + duration;
    }
}
