using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookToGrapplePoints : MonoBehaviour
{
    public ThirdPersonMovement tpm;
    public CharacterController controller;
    //Hook
    public LineRenderer lr;
    private Vector3 currentGrapplePosition;
    public Transform armTip;
    public Camera mainCam;

    public float waveCount;
    public float waveHeight;

    public int quality;

    public float damper;
    public float strength;
    public float velocity;

    public AnimationCurve affectCurve;
    private Spring spring;
    public AnimationCurve hookCurve;

    private bool isDrawingRope = false;
    public float hookSpeed = 5f;
    static float closestDistance;
    public List<GameObject> grapples;
    public bool isHooked = false;
    private GameObject closest;
    private GameObject hookedPoint;
    public Animator animator;


    private float currentAnimTime, totalAnimTime;

    private void Awake()
    {
        spring = new Spring();
        spring.SetTarget(0);
    }
    void Start()
    {
        totalAnimTime = hookCurve.keys[hookCurve.keys.Length - 1].time;
    }

    // Update is called once per frame
    void Update()
    {
        //Hook input
        if (Input.GetKeyDown(KeyCode.F) && !isHooked && closest.GetComponent<GrapplePoint>().isHookable && tpm.isHookedToGrapplePoints)
        {
            //Hook
            hookedPoint = closest; // save hook point for reference
            GrapplePoint gp = closest.GetComponent<GrapplePoint>();
            gp.StartActivationAnimation();
            animator.SetTrigger("Hook");

        }
        HookToGrapplePoint();


        //find grapple point in world
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, 200f);
        float dot = 0.5f;
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "GrapplePoint")
            {
                // add all grapple point in List
                GameObject grapple = hitCollider.gameObject;
                if (!grapples.Contains(grapple))
                {
                    grapples.Add(grapple);
                }
            }
        }

        for (int i = 0; i < grapples.Count; i++)
        {
            //Find closest grapple point to player to show up only one point
            Vector3 localpoint = mainCam.transform.InverseTransformPoint(grapples[i].transform.position).normalized;
            Vector3 forward = Vector3.forward;
            float test = Vector3.Dot(localpoint, forward);
            if (test > dot)
            {
                dot = test;
                closestDistance = test;
                closest = grapples[i].gameObject;
            }
        }
        if (isHooked)
            closestDistance = Vector3.Distance(gameObject.transform.position, hookedPoint.transform.position);
        else
            closestDistance = Vector3.Distance(gameObject.transform.position, closest.transform.position);
    }


    private void HookToGrapplePoint()
    {
        if (isHooked)
        {
            if (hookedPoint.GetComponent<GrapplePoint>().isHookable)
            {
                if (closestDistance >= 1f)
                {

                    //Movement to grapple Point
                    Transform timed;
                    timed = hookedPoint.gameObject.transform;
                    currentAnimTime = 0;
                    currentAnimTime += Time.deltaTime;
                    if (currentAnimTime >= totalAnimTime) currentAnimTime = 0;
                    Vector3 grappleDir = (timed.position - transform.position).normalized;
                    Quaternion lookRot = Quaternion.LookRotation(grappleDir);
                    lookRot.x = 0f;
                    lookRot.z = 0f;
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 10f);
                    grappleDir += new Vector3(0f, 0.5f, 0f);
                    controller.Move(grappleDir * Time.deltaTime * hookSpeed * hookCurve.Evaluate(currentAnimTime));
                    if (Mathf.Round(transform.position.y) != Mathf.Round(hookedPoint.transform.position.y)) tpm.isGrounded = false;
                }
                else
                {
                    isHooked = false;
                    tpm.mainIsHooked = false;
                }
            }
        }
    }

    private void LateUpdate()
    {
        float delta = Time.deltaTime;
        DrawRope();
        DeActivateGrapples();
        if (!isHooked) ActivateGrapples();
    }

    private void ActivateGrapples()
    {
        GrapplePoint gp = closest.GetComponent<GrapplePoint>();
        gp.Activate();
        if (closestDistance > 35f)
        {
            gp.DeActivate();
        }
    }
    //Animation Events for Grappling
    public void StartMovement()
    {
        isHooked = true;
        tpm.mainIsHooked = true;
    }

    public void StartDrawingRope()
    {
        isDrawingRope = true;
    }
    public void StopDrawingRope()
    {
        isDrawingRope = false;
    }
    private void DeActivateGrapples()
    {
        for (int i = 0; i < grapples.Count; i++)
        {
            GrapplePoint gp = grapples[i].GetComponent<GrapplePoint>();
            gp.DeActivate();
        }
    }
    public static float GetClosestDistance()
    {
        return closestDistance;
    }

    //Drawing Rope for Grappling 
    void DrawRope()
    {
        if (!isDrawingRope)
        {
            currentGrapplePosition = armTip.position;
            spring.Reset();
            if (lr.positionCount > 0)
                lr.positionCount = 0;
            return;
        }
        if (lr.positionCount == 0)
        {
            spring.SetVelocity(velocity);
            lr.positionCount = quality + 1;
        }
        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.Update(Time.deltaTime);

        var grapplePoint = hookedPoint.gameObject.transform.position;
        var gunTipPosition = armTip.position;
        var up = Quaternion.LookRotation((grapplePoint - gunTipPosition).normalized) * Vector3.up;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 12f);

        for (var i = 0; i < quality + 1; i++)
        {
            var delta = i / (float)quality;
            var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value *
                         affectCurve.Evaluate(delta);

            lr.SetPosition(i, Vector3.Lerp(gunTipPosition, currentGrapplePosition, delta) + offset);
        }
    }
}
