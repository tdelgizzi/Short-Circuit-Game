using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrappleManagerV2 : MonoBehaviour
{
    // Public API
    // On key down, call StartGrapple
    // This method is safe and can be called even if the grapple is still deployed.
    public void StartGrapple()
    {
        AttemptGrapple();
    }

    // On key up, call EndGrapple
    public void EndGrapple()
    {
        
    }
    // End Public API

    [SerializeField] GameObject GrapplePrefab;
    [SerializeField] GameObject RopeLinkPrefab;
    [SerializeField] GameObject Player;
    [SerializeField] float ShootSpeed;
    [SerializeField] float RecoilMagnitude;
    [SerializeField] float Z;

    private Rigidbody2D rb;

    private float ropeLength;
    private float selfRadius;
    private List<RopeLink> rope = new List<RopeLink>();

    private bool isGrappling = false;
    private bool isReeling = false;
    private GameObject grapple;
    private Hook hook;

    private LineRenderer lineRender;

    void Start()
    {
        rb = Player.GetComponent<Rigidbody2D>();
        ropeLength = RopeLinkPrefab.transform.localScale.y;
        selfRadius = Player.transform.localScale.x;

        lineRender = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (grapple == null || hook == null) return;

        // General computations

        if (hook.IsLocked())
        {
            //if (isReeling)
            //{
                ReelingUpdate();
            //}
            //else
            //{
                //HookedUpdate();
            //}
        }
        else
        {
            ShootingUpdate();
        }

        // Render
        var points = rope.Select(link => link.GetFront()).ToList();
        points.Add(Player.transform.position);
        //points.Add(grapple.transform.position);
        var newPoints = LineSmoother.SmoothLine(points.ToArray(), ropeLength);
        SetLineRender(newPoints);
    }

    private void ShootingUpdate()
    {
        var toRope = grapple.transform.position - Player.transform.position;
        if (rope.Count > 0) toRope = (rope[rope.Count - 1].transform.position - Player.transform.position);
        var distanceToRope = toRope.magnitude;
        if (distanceToRope >= selfRadius + ropeLength) InsertLink();
    }

    private void HookedUpdate()
    {
        if (rope.Count < 1 || hook == null) Reset();

        Vector3[] points = new Vector3[rope.Count + 1];
        points[0] = hook.GetRopeAttachmentPoint();
        for (int i = 1; i < points.Length; i++)
        {
            points[i] = rope[i - 1].GetBack();
        }

        var toRope = grapple.transform.position - Player.transform.position;
        if (rope.Count > 0) toRope = (rope[rope.Count - 1].transform.position - Player.transform.position);
        var closestPoint = Player.transform.position + toRope.normalized * selfRadius;
        FABRIK.Solve(points, closestPoint);

        for (int i = 0; i < rope.Count; i++)
        {
            var front = points[i];
            var back = points[i + 1];
            rope[i].SetFrontAndBack(front, back);
        }

        var connectionPoint = rope[rope.Count - 1].GetBack();
        var toConnectionPoint = connectionPoint - closestPoint;
        if (toConnectionPoint.magnitude > 0.1f)
        {
            rb.AddForce(toConnectionPoint * 100);
        }
    }


    private float reelingTime = 0;
    private float timeBetweenReels = 0.05f;
    private void ReelingUpdate()
    {
        if (reelingTime == 0) reelingTime = Time.time;
        if (Time.time >= reelingTime + timeBetweenReels)
        {
            reelingTime = Time.time;
            if (rope.Count > 2)
            {
                var ropeLink = rope[rope.Count - 1];
                rope.RemoveAt(rope.Count - 1);
                Destroy(ropeLink.gameObject);
                HookedUpdate();
            }
            else
            {
                Reset();
            }
        }
    }

    public void AttemptGrapple()
    {
        if (isGrappling)
        {
            if (isReeling)
            {
                Reset();
            }
            else
            {
                isReeling = true;
            }
        }
        else
        {
            SpawnGrapplingPrefab();
            isGrappling = true;
        }
    }

    private void SpawnGrapplingPrefab()
    {
        var spawn_location = Player.transform.position + Player.transform.up.normalized * 0.5f;
        spawn_location.z = Z;
        grapple = Instantiate(GrapplePrefab, spawn_location, Quaternion.identity);
        grapple.transform.up = Player.transform.up;

        // Velocity
        var grapple_rb = grapple.GetComponent<Rigidbody2D>();
        grapple_rb.velocity = (Vector2) Player.transform.up.normalized * ShootSpeed;

        // Impact player with shot
        var rb = Player.GetComponent<Rigidbody2D>();
        rb.velocity -= (Vector2) Player.transform.up.normalized * RecoilMagnitude;

        hook = grapple.GetComponent<Hook>();
    }

    private void InsertLink()
    {
        var link = Instantiate(RopeLinkPrefab, Player.transform.position, Quaternion.identity);
        var ropeLink = link.GetComponent<RopeLink>();
        ropeLink.z = Z;
        if (rope.Count == 0)
        {
            ropeLink.SetTrack(hook);
        }
        else
        {
            ropeLink.SetTrack(rope[rope.Count - 1]);
        }
        rope.Add(ropeLink);
    }

    private void CleanUp()
    {
        Destroy(grapple);
        grapple = null;
        hook = null;
        foreach (RopeLink link in rope)
        {
            Destroy(link.gameObject);
        }
        rope.Clear();
        SetLineRender(new Vector3[0]);
    }

    private void Reset()
    {
        CleanUp();
        isGrappling = false;
        isReeling = false;
    }

    private void SetLineRender(Vector3[] points)
    {
        lineRender.positionCount = points.Length;
        lineRender.SetPositions(points);
    }
}
