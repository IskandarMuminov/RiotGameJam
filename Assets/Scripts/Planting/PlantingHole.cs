using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlantingHole : MonoBehaviour
{

    [SerializeField] private Dirt[] dirtObjects;
    [SerializeField] private bool inHole = false;
    [SerializeField] private bool isPlanted = false;
    public static UnityAction plantingCompleted; 

    private void OnEnable()
    {
        Dirt.dirt_moved += CheckIfBuried;
    }

    public void setInHole(bool value)
    {
        inHole = value;

    }

    private void CheckIfBuried()
    {
        Debug.Log("Check if Buried"); 
        // If player is positioned in the hole
        if (inHole)
        {
            // check if all dirt is at the start position
            foreach (Dirt dirt in dirtObjects)
            {
                // if false return 
                if (!dirt.atStart)
                {
                    return;
                }
            }
            // if all true, plant is planted 
            isPlanted = true;
            plantingCompleted?.Invoke();

        }
    }

}
