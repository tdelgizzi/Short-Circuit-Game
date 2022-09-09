using System.Collections.Generic;
using UnityEngine;

public class Robots : MonoBehaviour
{
    [SerializeField] GameObject FullRobotContainerPrefab;
    [SerializeField] GameObject OpenEmptyContainerPrefab;
    [SerializeField] GameObject ClosedEmptyContainerPrefab;

    private List<GameObject> containers = new List<GameObject>();
    private int spawnIndex = 0;

    void Start()
    {
        foreach (Transform child in transform)
        {
            containers.Add(child.gameObject);
        }
    }

    void Update()
    {

    }

    public void UseNextRobot()
    {
        UpdateContainer(ClosedEmptyContainerPrefab, spawnIndex);
        spawnIndex += 1;
        UpdateContainer(OpenEmptyContainerPrefab, spawnIndex);
    }

    private void UpdateContainer(GameObject containerPrefab, int index)
    {
        containers.Insert(index, SpawnPrefab(containerPrefab, index));
        Destroy(containers[index + 1]);
        containers.RemoveAt(index + 1);
    }

    private GameObject SpawnPrefab(GameObject prefab, int index)
    {
        var obj = Instantiate(prefab, transform);
        obj.transform.position = GetPosition(index);
        return obj;
    }

    public Vector2 SpawnPosition()
    {
        return GetPosition(spawnIndex);
    }

    private Vector2 GetPosition(int index)
    {
        return containers[index].transform.position;
    }
}
