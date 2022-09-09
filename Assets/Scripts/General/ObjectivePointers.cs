using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectivePointers : MonoBehaviour
{
    [SerializeField] GameObject pointerPrefab;
    [SerializeField] GameObject objectivesParentObject;
    [SerializeField] GameObject player;
    [SerializeField] int numberToShow = 1;
    [SerializeField] float edgePadding = 5;
    [SerializeField] int currentBat = 1;

    private List<GameObject> objectives = new List<GameObject>();
    private List<GameObject> pointers = new List<GameObject>();

    private static Canvas canvas;

    private static float EdgePadding = 5;

    void Start()
    {
        // Find objectives
        foreach (Transform t in objectivesParentObject.transform)
        {
            objectives.Add(t.gameObject);
        }

        // Spawn number of indicators
        for (int i = 0; i < numberToShow; i++)
        {
            pointers.Add(SpawnPointer());
        }

        EdgePadding = edgePadding;

        canvas = transform.GetComponentInParent<Canvas>();
    }

    void Update()
    {
        // To make the inspector work
        EdgePadding = edgePadding;

        // Get objectives and expand/shrink pointers
        var closestGameObjects = FindNClosestActiveObjectives();
        FitPointersToSize(closestGameObjects.Count);

        // Show pointers on screen
        for (int i = 0; i < closestGameObjects.Count; i++)
        {
            var objectiveViewportPoint = Camera.main.WorldToViewportPoint(closestGameObjects[i].transform.position);
            if (IsPointInCamera(objectiveViewportPoint))
            {
                // It is inside the screen, do not show the pointer
                pointers[i].SetActive(false);
            }
            else
            {
                // It is outside the screen, show the pointer
                SetPointerLocation(pointers[i], closestGameObjects[i], EdgePadding);
                if (i < 0) SetPointerBodyScale(pointers[i]);
                pointers[i].SetActive(true);
            }
        }
    }

    // Adapted from this video: https://www.youtube.com/watch?v=gAQpR1GN0Os
    public static void SetPointerLocation(GameObject pointer, GameObject target, float edgePadding)
    {
        // All units are world coordinates!
        var cameraCenter = Camera.main.transform.position;
        cameraCenter.z = 0;

        var upperRightCameraCorner = Camera.main.ViewportToWorldPoint(Vector3.one);
        upperRightCameraCorner.z = 0;

        var lowerLeftCameraCorner = Camera.main.ViewportToWorldPoint(Vector3.zero);
        lowerLeftCameraCorner.z = 0;

        var targetPosition = target.transform.position;
        targetPosition.z = 0;

        var screenWidth = (upperRightCameraCorner.x - cameraCenter.x) * 2;
        var screenHeight = (upperRightCameraCorner.y - cameraCenter.y) * 2;

        var rightEdgeBounds = new Bounds(new Vector3(upperRightCameraCorner.x, cameraCenter.y, 0), new Vector3(edgePadding, screenHeight, 1000));
        var leftEdgeBounds = new Bounds(new Vector3(lowerLeftCameraCorner.x, cameraCenter.y, 0), new Vector3(edgePadding, screenHeight, 1000));
        var topEdgeBounds = new Bounds(new Vector3(cameraCenter.x, upperRightCameraCorner.y, 0), new Vector3(screenWidth, edgePadding, 1000));
        var bottomEdgeBounds = new Bounds(new Vector3(cameraCenter.x, lowerLeftCameraCorner.y, 0), new Vector3(screenWidth, edgePadding, 1000));

        var ray = new Ray(cameraCenter, targetPosition - cameraCenter);

        float distance = float.PositiveInfinity;
        if (rightEdgeBounds.IntersectRay(ray, out distance)) {}
        else if (leftEdgeBounds.IntersectRay(ray, out distance)) {}
        else if (topEdgeBounds.IntersectRay(ray, out distance)) {}
        else if (bottomEdgeBounds.IntersectRay(ray, out distance)) {}

        if (distance != float.PositiveInfinity)
        {
            var point = ray.origin + ray.direction * distance;
            point.z = 0;

            Debug.DrawLine(cameraCenter, point);

            // Set screen location
            var screenPoint = Camera.main.WorldToScreenPoint(point) / canvas.scaleFactor;
            pointer.GetComponent<RectTransform>().anchoredPosition = screenPoint;

            // Set screen rotation
            float angle = Mathf.Atan2(targetPosition.y - cameraCenter.y, targetPosition.x - cameraCenter.x) - 90 * Mathf.Deg2Rad;
            pointer.transform.localRotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
        }
    }

    private static void SetPointerBodyScale(GameObject pointer)
    {
        var pointerBody = pointer.transform.GetChild(0);
        if (!pointerBody) return;
        var pointerBodyRectTransform = pointerBody.GetComponent<RectTransform>();
        if (!pointerBodyRectTransform) return;
        pointerBodyRectTransform.localScale = Vector2.one * 0.65f;
    }

    public static bool IsPointInCamera(Vector2 viewportPoint)
    {
        return viewportPoint.x <= 1 && viewportPoint.x >= 0 && viewportPoint.y <= 1 && viewportPoint.y >= 0;
    }

    private void FitPointersToSize(int size)
    {
        if (pointers.Count > size)
        {
            for (int i = size; i < pointers.Count; i++)
            {
                Destroy(pointers[i]);
            }
            pointers = pointers.Take(size).ToList();
        }
        else if (pointers.Count < size)
        {
            for (int i = pointers.Count; i < size; i++)
            {
                pointers.Add(SpawnPointer());
            }
        }
    }

    private GameObject SpawnPointer()
    {
        return Instantiate(pointerPrefab, transform);
    }

    private List<GameObject> FindNClosestActiveObjectives()
    {
        objectives = objectives.Where(x => x != null).ToList();
        return objectives
            .Where(x => x.activeSelf)
            //todo, change this to be the battery order, and add the battery order on the prefab
            //.OrderBy(x => (player.transform.position - x.transform.position).magnitude)
            .OrderBy(x => x.transform.GetComponent<ObjectiveNumber>().getObjectiveNum())
            .Take(numberToShow)
            .ToList();
    }
}
