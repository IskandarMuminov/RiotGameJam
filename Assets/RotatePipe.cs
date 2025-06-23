using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePipe : MonoBehaviour
{
    public GameObject RotatedPipe;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (RotatedPipe)
            {
                RotatedPipe.transform.rotation = Quaternion.Euler(
                    RotatedPipe.transform.rotation.eulerAngles.x,
                    RotatedPipe.transform.rotation.eulerAngles.y + 90f,
                    RotatedPipe.transform.rotation.eulerAngles.z
                );
            }

        }
        
    }
}
