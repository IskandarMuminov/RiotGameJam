using KinematicCharacterController;
using UnityEngine;

public class GrapplingGun : MonoBehaviour {
    public LineRenderer lineRenderer;
    //public AudioSource grappleSound;
   // public AudioSource ungrappleSound;
    private Vector3 grapplePoint;
    public bool isGrappling;
    public bool canGrapple;
    public bool grabbableObject;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, cameraPlayer, player;
    private float maxDistance = 100f;
    private SpringJoint joint;
    [Header("AimAssist")]
    public float aimAssistSize = 1f;
    public GameObject debugAssist;
    //public AudioSource aimAssistSound;
    [Header("Grappling Settings")]
    public KeyCode GrapplingKey = KeyCode.Mouse0;
    public float spring;
    public float damper;
    public float massScale;
    public KinematicCharacterMotor motor;

    void Start()
    {
        canGrapple = true;
    }
    void Update() {
        if (Input.GetKeyDown(GrapplingKey)) {
            StartGrapple();
        }
        else if (Input.GetKeyUp(GrapplingKey)) {
            StopGrapple();
        }
        if (cameraPlayer == null)
        {
            return;
        }
        RaycastHit hit;
        if (Physics.SphereCast(cameraPlayer.position, aimAssistSize, cameraPlayer.forward, out hit, maxDistance, whatIsGrappleable))
        {
            debugAssist.SetActive(true);
            debugAssist.transform.position = hit.point;
            //aimAssistSound.mute = false;
        }
        else
        {
            //aimAssistSound.Play();
            debugAssist.SetActive(false);
            //aimAssistSound.mute = true;
        }
    }

    //Called after Update
    void LateUpdate() {
        DrawRope();
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartGrapple() {
        RaycastHit hit;
        if (Physics.SphereCast(cameraPlayer.position, aimAssistSize, cameraPlayer.forward, out hit, maxDistance, whatIsGrappleable))
        {
            canGrapple = false;
            grapplePoint = hit.point;
            //grappleSound.Play();
            isGrappling = true;
            if (hit.collider.tag == "Grabbable")
            {
                grabbableObject = true;
                canGrapple = true;
            }
            if (grabbableObject == false)
            {
                joint = player.gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = grapplePoint;
            }
            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //The distance grapple will try to keep from grapple point. 
            if (grabbableObject == false)
            {
                joint.maxDistance = distanceFromPoint * 0.8f;
                joint.minDistance = distanceFromPoint * 0.25f;
                canGrapple = false;
                joint.spring = spring;
                joint.damper = damper;
                joint.massScale = massScale;
            }

            //Adjust these values to fit your game.
        }
        else
            isGrappling = false;
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple() {
        if (isGrappling == true)
        {
            //ungrappleSound.Play();
            isGrappling = false;
            Destroy(joint);
            //aimAssistSound.Stop();
            grabbableObject = false;
            canGrapple = true;
        }
        isGrappling = false;
        Destroy(joint);
        //aimAssistSound.Stop();
        grabbableObject=false;
    }
    void FixedUpdate()
{
    if (isGrappling && !grabbableObject)
    {
        Vector3 toGrapple = grapplePoint - motor.TransientPosition;
        float dist = toGrapple.magnitude;
        Vector3 dir = toGrapple.normalized;

        float force = (dist * spring) - Vector3.Dot(motor.BaseVelocity, dir) * damper;
        motor.BaseVelocity += dir * force * Time.fixedDeltaTime;
    }
}

    private Vector3 currentGrapplePosition;
    
    void DrawRope() {
        //If not grappling, don't draw rope
        if (!joint) return;
    }
    

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint() {
        return grapplePoint;
    }
}
