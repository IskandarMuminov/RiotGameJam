using UnityEngine;

public class GrappleRope : MonoBehaviour {
    public AudioSource grappleSound;
    public AudioSource ungrappleSound;
    private Vector3 grapplePoint;
    public bool isGrappling;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, cameraPlayer, player;
    private float maxDistance = 100f;
    private SpringJoint joint;
    [Header("AimAssist")]
    public float aimAssistSize = 1f;
    public GameObject debugAssist;
    public AudioSource aimAssistSound;
    [Header("Grappling Settings")]
    public KeyCode GrapplingKey = KeyCode.Mouse0;
    public float spring;
    public float damper;
    public float massScale;


    void Update()
    {
        if (Input.GetKeyDown(GrapplingKey))
        {
            StartGrapple();
        }
        else if (Input.GetKeyUp(GrapplingKey))
        {
            StopGrapple();
        }

        RaycastHit hit;
        if (Physics.SphereCast(cameraPlayer.position, aimAssistSize, cameraPlayer.forward, out hit, maxDistance, whatIsGrappleable))
        {
            debugAssist.SetActive(true);
            debugAssist.transform.position = hit.point;
            aimAssistSound.mute = false;
        }
        else
        {
            aimAssistSound.Play();
            debugAssist.SetActive(false);
            aimAssistSound.mute = true;
        }
    }

    //Called after Update
    void LateUpdate()
    {
        DrawRope();
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.SphereCast(cameraPlayer.position, aimAssistSize, cameraPlayer.forward, out hit, maxDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            grappleSound.Play();
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;
            isGrappling = true;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Adjust these values to fit your game.
            joint.spring = spring;
            joint.damper = damper;
            joint.massScale = massScale;
        }
        else
            isGrappling = false;
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple()
    {
        if (isGrappling == true)
        {
            ungrappleSound.Play();
        }
        isGrappling = false;
        Destroy(joint);
        aimAssistSound.Stop();
    }

    private Vector3 currentGrapplePosition;

    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!joint) return;
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
