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
    public GameObject player;

    void Start()
    {
        startPos = transform.localPosition;
        target = startPos;
        player = GameObject.FindGameObjectWithTag("Player"); 
        //Debug.Log("Start"); 
        //Move();
    }

    void FixedUpdate()
    {
        if (transform.position != target)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, 0.1f); 
        }
    }

    public void Move()
    {
        Debug.Log(player.GetComponentInParent<ResourceManager>().player_State);

        if (player.GetComponentInParent<ResourceManager>().player_State == Player_State.Mature)
        {
            if (atStart)
            {
                target = endPos;
                atStart = false;
                // disable colliders
                GetComponent<SphereCollider>().enabled = false;
            }
            else
            {
                target = startPos;
                atStart = true;

            }
            dirt_moved?.Invoke();
        }

    }
    
}
