using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Dirt : MonoBehaviour
{
    private Vector3 startPos;
    [SerializeField] private Vector3 endPos;
    public bool atStart = true;
    private Vector3 target;

    public static UnityAction dirt_moved;

    void Start()
    {
        startPos = transform.position;
        target = startPos; 
        //Debug.Log("Start"); 
        //Move();
    }

    void FixedUpdate()
    {
        if (transform.position != target)
        {
            transform.position = Vector3.Lerp(transform.position, target, 0.1f); 
        }
    }

    public void Move()
    {
        Debug.Log("At Start: " + atStart);
        dirt_moved?.Invoke(); 
        if (atStart)
        {
            target = endPos;
            atStart = false;
        }
        else
        {
            target = startPos;
            atStart = true;
        }
    }
    
}
