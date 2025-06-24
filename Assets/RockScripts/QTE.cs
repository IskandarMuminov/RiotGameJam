using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Qte : MonoBehaviour
{
    public Transform pointA; // Reference to the starting point
    public Transform pointB; // Reference to the ending point
    public RectTransform safeZone; // Reference to the safe zone RectTransform
    private float moveSpeed; // Speed of the pointer movement
    public UnityEvent onSuccess;
    public UnityEvent onFailure;
    public UnityEvent onDestroyEvent;
    private float direction = 1f; // 1 for moving towards B, -1 for moving towards A
    private RectTransform pointerTransform;
    private Vector3 targetPosition;
    
 
    void Start()
    {
        pointerTransform = GetComponent<RectTransform>();
        targetPosition = pointB.position;
        moveSpeed = Random.Range(4, 8);
    }
 
    void Update()
    {
        // Move the pointer towards the target position
        pointerTransform.position = Vector3.MoveTowards(pointerTransform.position, targetPosition, moveSpeed * Time.deltaTime);
 
        // Change direction if the pointer reaches one of the points
        if (Vector3.Distance(pointerTransform.position, pointA.position) < 0.1f)
        {
            targetPosition = pointB.position;
            direction = 1f;
        }
        else if (Vector3.Distance(pointerTransform.position, pointB.position) < 0.1f)
        {
            targetPosition = pointA.position;
            direction = -1f;
        }
 
        // Check for input
        if (Input.GetMouseButton(0))
        {
            CheckSuccess();
        }
    }
 
    void CheckSuccess()
    {
        // Check if the pointer is within the safe zone
        if (RectTransformUtility.RectangleContainsScreenPoint(safeZone, pointerTransform.position, null))
        {
            onSuccess.Invoke();
            Debug.Log("Success!");
        }
        else
        {
            onFailure.Invoke();
            //dangerZone.TakeDamage();
            Debug.Log("Fail!");
        }
    }

    private void OnDestroy()
    {
        onDestroyEvent.Invoke();
    }
}
