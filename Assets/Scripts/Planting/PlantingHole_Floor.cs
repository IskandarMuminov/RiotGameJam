using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantingHole_Floor : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter: " + other.gameObject.tag);
        // if colliding with player
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Collide with Player");

            GetComponentInParent<PlantingHole>().setInHole(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // if colliding with player
        if (other.gameObject.tag == "Player")
        {
            GetComponentInParent<PlantingHole>().setInHole(false);
        }
    }
}
