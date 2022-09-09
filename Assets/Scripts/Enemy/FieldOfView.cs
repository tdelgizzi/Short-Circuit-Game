using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FIELD OF VISION - 2D Version
// Adapted from Sebastian Lague's FOV Guide
// by Ranadeep Mitra
//
// This script can be used on a gameobject to detect target
// colliders within a conical field of vision. The script
// respects specified wall colliders in target detection,
// allowing vision to be impeded by colliders.
// 
// This script populates the 'visibleTargets' list with
// every visible target within the conical field of view
// as long as there is not a wall between the object and
// the target. The 'targetMask' determines which objects
// are detected by the script.

public class FieldOfView : MonoBehaviour
{
    // If false, the center of the target collider must be within
    // the field of vision cone to be detected.
    // If true, the script will check for targets on the edges
    // of the field of vision cone. 
    public bool detectTargetOnEdges = false;

    // Offset for FOV calculations
    public Vector3 offset;

    // The radius or distance from center the target can be.
    public float viewRadius;

    // The angle in front of the gameObject that the target is
    // detected within.
    [Range(0, 360)]
    public float viewAngle;

    // LayerMask of colliders than can break vision.
    public LayerMask wallMask;
    // LayerMask of colliders that are detected as targets.
    public LayerMask targetMask;


    // --- VISION MESH ---

    // Will the script update the field of vision mesh?
    public bool drawFov = false;

    // The number of vertices that are calculated per angle.
    public float viewResolution;

    // Number of times the edge resolution algorithm will be
    // run before selecting midpoint.
    public int edgeResolveIterations;

    // Distance threshold of edge resolution.
    public float edgeDistThreshold;

    // A non-zero value will cause the vision mesh to extend past
    // a collision with a wall to the specified distance. This
    // will cause the mesh to render beyond the wall collision.
    // Note: this does not affect target detection at all.
    public float maskDist = 0f;

    // Field of Vision Visualization Mesh
    public GameObject ViewMeshGameobject;

    MeshFilter viewMeshFilter;
    Mesh viewMesh;

    // Running list of all detected targets
    // Uncomment [HideInInspector] to stop showing the list of
    // detected targets in the inspector.
    // [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    void Start()
    {
        if (viewMeshFilter != null)
        {
            viewMeshFilter = ViewMeshGameobject.GetComponent<MeshFilter>();
            viewMesh = new Mesh
            {
                name = "View Mesh"
            };
            viewMeshFilter.mesh = viewMesh;
            viewMeshFilter.gameObject.transform.position += offset;
        }


        StartCoroutine(PopulateTargets(0.1f));
    }

    private void LateUpdate()
    {
        if (drawFov && viewMeshFilter != null)
            DrawFOV();
    }

    // Target Loop
    IEnumerator PopulateTargets(float delay)
    {
        while (true)
        {
            PopulateTargetsInVision();

            yield return new WaitForSeconds(delay);
        }
    }

    void PopulateTargetsInVision()
    {
        // Clear Visible Targets List
        visibleTargets.Clear();

        // Array of all target colliders in the radius.
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position + offset, viewRadius, targetMask);

        // Check that the object has clear vision of the target, unimpeded by a wall.
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.up, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position + offset, target.position);

                RaycastHit2D hit = Physics2D.Raycast(transform.position + offset, dirToTarget, dstToTarget, wallMask);
                if (hit.collider == null)
                {
                    visibleTargets.Add(target);
                }
            }
            else if (detectTargetOnEdges)
            {
                int fullMask = wallMask.value | targetMask.value;
                RaycastHit2D hit = Physics2D.Raycast(transform.position + offset, DirFromAngle(-viewAngle / 2, false), viewRadius, fullMask);

                if (hit.collider != null)
                {
                    if (hit.collider.Equals(targetsInViewRadius[i]))
                        visibleTargets.Add(target);
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angle, bool globalAngle)
    {
        if (!globalAngle)
            angle += -transform.eulerAngles.z;

        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad),
                           Mathf.Cos(angle * Mathf.Deg2Rad),
                           0);
    }

    // --- VISION MESH ---

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        // Constructor
        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    void DrawFOV()
    {
        int numSteps = Mathf.RoundToInt(viewAngle * viewResolution);
        float stepAngleSize = viewAngle / numSteps;

        List<Vector3> viewPoints = new List<Vector3>();

        // Edge Detection Struct
        ViewCastInfo oldViewCast = new ViewCastInfo();

        // Find all the viewpoints
        for (int i = 0; i <= numSteps; i++)
        {
            float angle = -transform.eulerAngles.z - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo vc = ViewCast(angle);

            // Edge Detection Logic
            if (i > 0)
            {

                bool edgeDistThresholdExceeded = Mathf.Abs(oldViewCast.dst - vc.dst) > edgeDistThreshold;
                if (oldViewCast.hit != vc.hit || (oldViewCast.hit && vc.hit && edgeDistThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, vc);
                    if (edge.a != Vector3.zero)
                    {
                        viewPoints.Add(edge.a);
                    }
                    if (edge.b != Vector3.zero)
                    {
                        viewPoints.Add(edge.b);
                    }
                }

            }

            viewPoints.Add(vc.point);
            oldViewCast = vc;
        }

        // Mesh creation logic
        // The number of vertices is equal to the number of rays shot out
        // plus one for the origin transform. These vertices are joined into
        // [vertexCount - 2] triangles. To programatically instantiate a mesh,
        // we need a int array of triangle vertices. 
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]) + Vector3.up * maskDist;

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + offset, dir, viewRadius, wallMask);
        if (hit.collider != null)
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, (transform.position + offset) + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    // Smooth Edge Detection

    public struct EdgeInfo
    {
        public Vector3 a;
        public Vector3 b;

        // Constructor
        public EdgeInfo(Vector3 _a, Vector3 _b)
        {
            a = _a;
            b = _b;
        }
    }
    
    // Finds edges by finding a vector halfway between the minViewCast and maxViewCast
    // vectors. These vectors are then moved each iteration depending on the whether the
    // halfway vector detected a collider or not. The number of iterations is dictated by
    // 'edgeResolveIterations'
    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDistThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDistThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDistThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }
}
