using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HookToMousePoint : MonoBehaviour
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
    public bool isHooked = false;
    private Vector3 closest;
    private Vector3 hookedPoint;
    public Animator animator;
    public Image crosshairImage;
    public Color crosshairColor;
    public Color crosshairHitColor;

    RaycastHit hit;
    float distanceToMousePosition;
    public float maxDistanceToMousePosition;
    public float minDistanceToDisableHookBoolean = 1f;
    private float saveMinDistanceVariable;
    private bool isHookingRightNow = false;

    private float currentAnimTime, totalAnimTime;

    private void Awake()
    {
        spring = new Spring();
        spring.SetTarget(0);
    }
    void Start()
    {
        saveMinDistanceVariable = minDistanceToDisableHookBoolean;
        totalAnimTime = hookCurve.keys[hookCurve.keys.Length - 1].time;
    }

    // Update is called once per frame
    void Update()
    {
        //Hook input
        //Find a point where player look
        var CameraCenter = mainCam.ScreenToWorldPoint(new Vector3(Screen.width/2f, Screen.height/2f,mainCam.nearClipPlane));
        //Cast a ray to that point
        Physics.Raycast(CameraCenter, mainCam.transform.forward, out hit, 1000);
        //Find a distance between player and that point
        distanceToMousePosition = Vector3.Distance(gameObject.transform.position, hookedPoint);
        float distanceToHitPoint = Vector3.Distance(gameObject.transform.position, hit.point);
        //Change crosshair color when you can grapple
        if (distanceToHitPoint <= maxDistanceToMousePosition && hit.transform != null)
        {
            crosshairImage.color = crosshairHitColor;
        }
        else crosshairImage.color = crosshairColor;
        if (Input.GetKeyDown(KeyCode.F) && !isHooked && !tpm.isHookedToGrapplePoints && hit.transform != null && distanceToHitPoint <= maxDistanceToMousePosition)
        {
            //Hook           
            closest = hit.point;
            hookedPoint = closest; // save hook point for reference
            animator.SetTrigger("Hook");

        }
        HookToGrapplePoint();


    }


    private void HookToGrapplePoint()
    {
        if (isHooked)
        {
            //Move if you grappling a point which in range of maximum distance variable
            if (distanceToMousePosition <= maxDistanceToMousePosition)
            {
                //Move if you still not in range of minimum distance variable
                //Change minDistanceToDisableHookBoolean if you stucking or releasing too soon after grappling
                if (distanceToMousePosition >= minDistanceToDisableHookBoolean)                
                {
                    //Movement to grapple Point
                    Vector3 timed;
                    timed = hookedPoint;
                    currentAnimTime = 0;
                    currentAnimTime += Time.deltaTime;
                    if (currentAnimTime >= totalAnimTime) currentAnimTime = 0;
                    Vector3 grappleDir = (timed - transform.position).normalized;
                    Quaternion lookRot = Quaternion.LookRotation(grappleDir);
                    lookRot.x = 0f;
                    lookRot.z = 0f;
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 10f);
                    //Change new Vector3 in Y direction to change force of moving gameObject to the up while grappling
                    grappleDir += new Vector3(0f, 0.7f, 0f);
                    controller.Move(grappleDir * Time.deltaTime * hookSpeed * hookCurve.Evaluate(currentAnimTime));
                    //If your hooked point is too far to disable isHooked boolean by previous if statement then variable of minimum distance is doubled
                    if (isHookingRightNow && gameObject.transform.position.y != timed.y && Mathf.Round(controller.velocity.z) == 0 && Mathf.Round(controller.velocity.x) == 0)
                    {
                        minDistanceToDisableHookBoolean *= 2;
                    }

                }
                else 
                {
                    isHookingRightNow = false;
                    tpm.mainIsHooked = false;
                    isHooked = false;
                }
            }
            else
            {
                isHookingRightNow = false;
                tpm.mainIsHooked = false;
                isHooked = false;
            }
        }
    }

    private void LateUpdate()
    {
        float delta = Time.deltaTime;
        DrawRope();
    }

    //Animation Events for Grappling
    public void StartMovement()
    {
        //return minimum distance variable to the first number
        minDistanceToDisableHookBoolean = saveMinDistanceVariable;
        isHooked = true;
        tpm.mainIsHooked = true;
        tpm.isGrounded = false;
        isHookingRightNow = true;
    }

    public void StartDrawingRope()
    {
        isDrawingRope = true;
    }
    public void StopDrawingRope()
    {
        isDrawingRope = false;
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

        var grapplePoint = hookedPoint;
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
