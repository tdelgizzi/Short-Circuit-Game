using System.Collections.Generic;
using UnityEngine;

// Create a wrapper to make the inner corners easier to work with
public class GrappleWrapper
{

    private Transform player;
    private List<Vector3> innerCorners = new List<Vector3>();
    private Transform hook;

    public GrappleWrapper(Transform player, Transform hook)
    {
        this.player = player;
        this.hook = hook;
    }

    public int Count { get { return 2 + innerCorners.Count; } }

    public bool HasInnerCorners()
    {
        return innerCorners.Count != 0;
    }

    public Vector3 this[int i]
    {
        get { return GetIndex(i); }
        set { SetIndex(i, value); }
    }

    private Vector3 GetIndex(int i)
    {
        var count = Count;
        if (i == 0) return GetPlayerRopeConnection();
        else if (i == count - 1) return GetHookRopeConnection();
        else if (i >= count || i < 0) throw new System.Exception("INDEX OUT OF BOUNDS");
        else return innerCorners[i - 1];
    }

    private void SetIndex(int i, Vector3 vec)
    {
        var count = Count;
        if (i == 0) throw new System.Exception("CANNOT ALTER INDEX 0");
        else if (i == count - 1) throw new System.Exception("CANNOT ALTER LAST INDEX");
        else if (i >= count || i < 0) throw new System.Exception("INDEX OUT OF BOUNDS");
        else
        {
            innerCorners[i - 1] = vec;
        }
    }

    public void InsertAt(int i, Vector3 vec)
    {
        var count = Count;
        if (i == 0) throw new System.Exception("CANNOT INSERT AT INDEX 0");
        else if (i >= count || i < 0) throw new System.Exception("INDEX OUT OF BOUNDS");
        else
        {
            innerCorners.Insert(i - 1, vec);
        }
    }

    public void InsertAllAt(int i, List<Vector3> vecs)
    {
        var count = Count;
        if (i == 0) throw new System.Exception("CANNOT INSERT AT INDEX 0");
        else if (i >= count || i < 0) throw new System.Exception("INDEX OUT OF BOUNDS");
        else
        {
            vecs.Reverse();
            foreach (Vector3 vec in vecs) innerCorners.Insert(i - 1, vec);
        }
    }

    public void RemoveAt(int i)
    {
        var count = Count;
        if (i == 0) throw new System.Exception("CANNOT ALTER INDEX 0");
        else if (i == count - 1) throw new System.Exception("CANNOT ALTER LAST INDEX");
        else if (i >= count || i < 0) throw new System.Exception("INDEX OUT OF BOUNDS");
        else
        {
            innerCorners.RemoveAt(i - 1);
        }
    }

    public void RemoveFirstCorner()
    {
        innerCorners.RemoveAt(0);
    }

    public void RemoveLastCorner()
    {
        innerCorners.RemoveAt(innerCorners.Count - 1);
    }

    public Vector3[] GetAll()
    {
        var positions = new Vector3[innerCorners.Count + 2];
        positions[0] = GetPlayerRopeConnection();
        for (int i = 0; i < innerCorners.Count; i++) positions[i + 1] = innerCorners[i];
        positions[positions.Length - 1] = GetHookRopeConnection();
        return positions;
    }

    public Vector3 GetPlayerRopeConnection()
    {
        var playerPosition = player.transform.position; //+ player.transform.up * spawnDistanceInFrontOfPlayer;
        return playerPosition;
    }

    public Vector3 GetHookRopeConnection()
    {
        var hookPosition = hook.transform.position - player.transform.up * 0.25f;
        return hookPosition;
    }
}
