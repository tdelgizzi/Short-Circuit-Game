using UnityEngine;

public class GrappleManagerV3 : MonoBehaviour
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
    [SerializeField] LayerMask RopeCollisions;
    [SerializeField] float GrappleCornerDistance;

    private GameObject grappleInstance;
    private LineRenderer lineRenderer;
    private GameObject hitTarget;

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
        EventBus.Subscribe<PlayerDiedEvent>(e => EndGrapple());
    }

    void FixedUpdate()
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
            var raycastHit = Raycast(start, end);
            if (raycastHit.collider != null)
            {
                var normal = Vector3.Cross(end - start, Vector3.forward).normalized;
                var newCorner = (Vector3) raycastHit.point + normal * GrappleCornerDistance;

                // Attempt to correct the normal direction (in or out of wall)
                var newRaycastHit = Raycast(start, newCorner);
                if (newRaycastHit.collider != null) newCorner = raycastHit.point - (Vector2)normal * GrappleCornerDistance;
                newRaycastHit = Raycast(start, newCorner);
                if (newRaycastHit.collider != null)
                {
                    EndGrapple();
                    return;
                }

                newCorner.z = -0.5f;
                grapple.InsertAt(i + 1, newCorner);
                i++;
            }
        }

        // Remove First Corner
        if (grapple.Count > 2)
        {
            var raycastHit = Raycast(grapple[0], grapple[2]);
            if (raycastHit.collider == null) grapple.RemoveFirstCorner();
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
            /*            
            if (targetToLastCorner.magnitude < 1.5f)
            {
                if (!grapple.HasInnerCorners()) CleanUp();
                else grapple.RemoveLastCorner();
                return;
            }
            */

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

    private RaycastHit2D Raycast(Vector3 start, Vector3 end)
    {
        var startToEnd = end - start;
        var raycasthit = Physics2D.Raycast(start, startToEnd.normalized, startToEnd.magnitude - 0.1f, RopeCollisions);
        return raycasthit;
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
        //CleanUp();
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
        rb.velocity = inertiaComponent + (Vector2)shootComponent;
    }

    private void RecoilPlayer()
    {
        var rb = Player.GetComponent<Rigidbody2D>();
        rb.AddForce(Player.transform.up.normalized * -PlayerRecoilForce);
    }
}
