using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshFacade : MonoBehaviour
{
    private static NavMeshFacade _instance;
    public static NavMeshFacade Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    [SerializeField] float NavMeshPlaneZPosition;

    [SerializeField] float PlayerPlaneZPosition;

    // Public

    public List<Vector3> GetPath(Vector3 start, Vector3 end)
    {
        var origin = ProjectOnNavMeshPlane(start);
        var destination = ProjectOnNavMeshPlane(end);

        NavMeshPath path = new NavMeshPath();
        try
        {
            NavMesh.CalculatePath(origin, destination, NavMesh.AllAreas, path);
            return new List<Vector3>(path.corners).Select(x => ProjectOnPlayerPlane(x)).ToList();
        }
        catch
        {
            return new List<Vector3>();
        }
    }

    public bool NavMeshIsBlocked(Vector3 start, Vector3 end)
    {
        var origin = ProjectOnNavMeshPlane(start);
        var destination = ProjectOnNavMeshPlane(end);
        return NavMesh.Raycast(origin, destination, out _, NavMesh.AllAreas);
    }

    // Private

    private Vector3 ProjectOnNavMeshPlane(Vector3 vec)
    {
        // We technically do not need to project onto the nav mesh plane before sampling the position
        // It should provide performance benefits, although
        var idealNavMeshPosition = VectorWithZ(vec, NavMeshPlaneZPosition);
        NavMeshHit hit;
        NavMesh.SamplePosition(idealNavMeshPosition, out hit, 2.0f, NavMesh.AllAreas);
        if (!hit.hit)
        {
            return vec;
        }
        return hit.position;
    }
    private Vector3 ProjectOnPlayerPlane(Vector3 vec)
    {
        return VectorWithZ(vec, PlayerPlaneZPosition);
    }
    private Vector3 VectorWithZ(Vector3 vec, float z)
    {
        vec.z = z;
        return vec;
    }
}
