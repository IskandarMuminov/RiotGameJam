using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
    //Script for GrapplePoints
    private MeshRenderer meshRender;
    private Animator anim;

    private float closestDistance;
    public  float distanceToGreen = 20f;
    public  float distanceToActivate = 8f;
    public  float distanceToHook = 3f;
    public bool isHookable = false;
    private void Awake()
    {
        meshRender = GetComponent<MeshRenderer>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        closestDistance = HookToGrapplePoints.GetClosestDistance();
        if (meshRender.enabled)
        {
            if (closestDistance < distanceToActivate)
            {
                meshRender.material.color = Color.Lerp(Color.green, Color.black, closestDistance / distanceToGreen);
            }
        }
        if (closestDistance <= distanceToHook)
        {
            isHookable = true;
        }
        else isHookable = false;

    }

    public void StartActivationAnimation()
    {
        anim.SetTrigger("Activate");
    }
    public void Activate()
    {
        if (closestDistance > 4f)
        {
            meshRender.enabled = true;
            meshRender.material.color = Color.Lerp(Color.green, Color.black, closestDistance / distanceToGreen);
        }
        else
        {
            DeActivate();
        }
    }

    public void DeActivate()
    {
        meshRender.enabled = false;
    }
}
