using UnityEngine;

public class GrappleManager : MonoBehaviour
{
    // Public API
    // On key down, call StartGrapple
    // This method is safe and can be called even if the grapple is still deployed.
    public void StartGrapple()
    {
        switch (State)
        {
            case GrapplingState.None:
                SpawnGrapple();
                SetGrappleVelocity();
                RecoilPlayer();
                grapple = new GrappleWrapper(Player.transform, grappleInstance.transform);
                State = GrapplingState.Shooting;
                AudioManager.PlayClipNow("Grapple");
                EventBus.PublishAnalyticsEvent(new WeaponUsedAnalyticsEvent("Grapple"));
                break;
        }
    }

    // On key up, call EndGrapple
    public void EndGrapple()
    {
        if (State == GrapplingState.None) return;
        CleanUp();
    }
    // End Public API

    [SerializeField] GameObject GrapplePrefab;
    [SerializeField] GameObject Player;
    [SerializeField] float GrappleShootSpeed;
    [SerializeField] float SpawnDistanceInFrontOfPlayer;
    [SerializeField] float PlayerRecoilForce;
    [SerializeField] float GrapplePullingForce;
    [SerializeField] float GrappleCloseEnoughToCornerDistance;

    private GameObject grappleInstance;
    private LineRenderer lineRenderer;
    private GameObject hitTarget;

    //private List<Vector3> innerCorners = new List<Vector3>();
    private GrappleWrapper grapple = null;

    public enum GrapplingState
    {
        Shooting,
        Pulling,
        None
    }

    private GrapplingState State = GrapplingState.None;

    private void Awake()
    {
        EventBus.Subscribe<GrappleHitTargetEvent>(OnGrappleHitTarget);
        EventBus.Subscribe<GrappleOutsideOfCameraEvent>(OnGrappleOutsideOfCameraEvent);
    }

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        // Do not update if grappling hook does not exist
        if (State == GrapplingState.None) return;

        // Common computations
        var playerPosition = grapple.GetPlayerRopeConnection();

        // Add Corners
        for (int i = 0; i < grapple.Count - 1; i++)
        {
            var start = grapple[i];
            var end = grapple[i + 1];
            if (NeedToComplexifyRope(start, end))
            {
                var newCorners = NavMeshFacade.Instance.GetPath(start, end);
                newCorners.RemoveAt(0);
                newCorners.RemoveAt(newCorners.Count - 1);
                grapple.InsertAllAt(i + 1, newCorners);
                i += newCorners.Count;
            }
        }

        // Remove corners
        for (int i = 0; i < grapple.Count - 2; i++)
        {
            var start = grapple[i];
            var mid = grapple[i + 1];
            var end = grapple[i + 2];
            if (NeedToSimplifyRope(start, mid, end))
            {
                grapple.RemoveAt(i + 1);
                i -= 1;
            }
        }

        // Specialized update        
        if (State == GrapplingState.Pulling)
        {
            // Logic to remove corners when we are pulled through them
            var playerToFirstCorner = grapple[1] - playerPosition;
            if (playerToFirstCorner.magnitude < GrappleCloseEnoughToCornerDistance)
            {
                if (!grapple.HasInnerCorners()) CleanUp();
                else grapple.RemoveFirstCorner();
                return;
            }

            var targetToLastCorner = grapple[grapple.Count - 2] - hitTarget.transform.position;
            if (targetToLastCorner.magnitude < 1.5f)
            {
                if (!grapple.HasInnerCorners()) CleanUp();
                else grapple.RemoveLastCorner();
                return;
            }

            // General physics
            var playerRb = Player.GetComponent<Rigidbody2D>();
            var targetRb = hitTarget.GetComponent<Rigidbody2D>();
            var isAttachedToMovingObject = targetRb != null;

            // Player physics
            var playerForceMagnitude = isAttachedToMovingObject ? GrapplePullingForce / 2 : GrapplePullingForce;
            playerRb.AddForce(playerToFirstCorner.normalized * playerForceMagnitude);

            // Target object physics 
            if (isAttachedToMovingObject)
            {                
                targetRb.AddForce(targetToLastCorner.normalized * GrapplePullingForce / 2);
            }
        }

        RenderRope();
    }

    private bool NeedToComplexifyRope(Vector3 start, Vector3 end)
    {
        return NavMeshFacade.Instance.NavMeshIsBlocked(start, end);
    }

    private bool NeedToSimplifyRope(Vector3 start, Vector3 mid, Vector3 end)
    {
        // See if we can get directly from start to the end
        // If we cannot, no simplifications can be made
        if (NavMeshFacade.Instance.NavMeshIsBlocked(start, end)) return false;

        // There must be a straight line from start -> end
        // We need to find if there exists any NavMeshObstacle in the triangle formed by start, mid, end
        var numSamples = 10;
        var sampleChunk = (end - start) / numSamples;
        for (int i = 0; i < numSamples - 1; i++)
        {
            var samplePoint = start + sampleChunk * i;
            if (NavMeshFacade.Instance.NavMeshIsBlocked(mid, samplePoint)) return false;
        }

        return true;
    }

    private void OnGrappleHitTarget(GrappleHitTargetEvent obj)
    {
        if (State == GrapplingState.Shooting)
        {
            State = GrapplingState.Pulling;
            hitTarget = obj.Target;
        }
    }

    private void OnGrappleOutsideOfCameraEvent(GrappleOutsideOfCameraEvent obj)
    {
        CleanUp();
    }

    private void CleanUp()
    {
        Destroy(grappleInstance);
        grappleInstance = null;
        grapple = null;
        State = GrapplingState.None;

        lineRenderer.SetPositions(new Vector3[0]);
        lineRenderer.positionCount = 0;

        hitTarget = null;
    }

    private void RenderRope()
    {
        // Populate Line Renderer
        lineRenderer.positionCount = grapple.Count;
        lineRenderer.SetPositions(grapple.GetAll());
    }

    private void SpawnGrapple()
    {
        var spawn_position = Player.transform.position
            + Player.transform.up * SpawnDistanceInFrontOfPlayer;
        grappleInstance = Instantiate(GrapplePrefab, spawn_position, Quaternion.identity);
        grappleInstance.transform.up = Player.transform.up;
        grappleInstance.GetComponent<GrappleHook>().Player = Player;
    }

    private void SetGrappleVelocity()
    {
        var rb = grappleInstance.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogWarning("Grappling Hook instance does not have a rigidbody component to move.");
            return;
        }

        var inertiaComponent = Player.GetComponent<Rigidbody2D>().velocity;
        var shootComponent = Player.transform.up.normalized * GrappleShootSpeed;
        rb.velocity = inertiaComponent + (Vector2) shootComponent;
    }

    private void RecoilPlayer()
    {
        var rb = Player.GetComponent<Rigidbody2D>();
        rb.AddForce(Player.transform.up.normalized * -PlayerRecoilForce); // TODO: interface with player
    }
}
