using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linefollow : MonoBehaviour
{
    public LineRenderer rendererLine;
    public Transform pos1;
    public Transform pos2;
    public GrapplingGun grapplingScript;
    // Start is called before the first frame update
    void Start()
    {
        if(grapplingScript.isGrappling==true)
        rendererLine.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (grapplingScript.isGrappling == true)
        {
            rendererLine.SetPosition(0, pos1.position);
            rendererLine.SetPosition(1, pos2.position);
        }
    }
}
