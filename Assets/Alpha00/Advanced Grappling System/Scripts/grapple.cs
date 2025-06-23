using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grapple : MonoBehaviour
{
    public Transform player;
    private SpringJoint joint;
    public Transform gunTip;
    public Transform cameraPlayer;
    private bool grabbableObject;
    private bool detected;
    public GrapplingGun grapplingScript;
    public Rigidbody rb;
    public Transform mainObject;
    public float force = 3;
    public LineRenderer rendererLine;
    public float lineDrawSpeed = 1;
    private float maxDistance = 100f;
    private float aimAssistSize;
    public LayerMask whatIsGrabbable;
    Vector3 vector3;
    // Start is called before the first frame update
    void Start()
    {
        rendererLine = GetComponent<LineRenderer>();
        rendererLine.positionCount = 2;
        rendererLine.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        grabbableObject = grapplingScript.grabbableObject;
        grapplingScript.cameraPlayer = cameraPlayer;
        grapplingScript.aimAssistSize = aimAssistSize;
        RaycastHit hit;
        if (Physics.SphereCast(cameraPlayer.position, aimAssistSize, cameraPlayer.forward, out hit, maxDistance, whatIsGrabbable))
        {
            if (hit.collider.transform == mainObject)
            {
                detected = true;
                grabbableObject = true;
                Debug.Log("Adevarat");
            }       
        }
        if (grapplingScript.isGrappling == false)
        {
            detected = false;
            grabbableObject = false;
            rendererLine.enabled = false;
        }
        if (grabbableObject == true && detected == true && grapplingScript.isGrappling == true && grapplingScript.canGrapple == true)
        {
            //enable line renderer
            rendererLine.enabled = true;
            //make the object come towards you
            Vector3 f = player.position - transform.position;
            f = f.normalized;
            f = f * force;
            rb.AddForce(f);
            //set position of the line renderer
            rendererLine.SetPosition(0, gunTip.position);
            rendererLine.SetPosition(1, mainObject.position);
        }
    }
    void FixedUpdate()
    {

    }
}
