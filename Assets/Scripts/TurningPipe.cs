using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningPipe : MonoBehaviour
{
    [SerializeField] private int Counter = 0;

    public GameObject gm1;
    public GameObject gm2;
    public GameObject gm3;

    private GameObject currentGameObject ;

    public bool RightPipe;

    public GameObject CorrectPipe;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Counter >= 2)
            {
                Counter = 0;
            }
            else
            {
                Counter++;
            }
            if (Counter == 0)
            {
                gm3.SetActive(false);
                gm1.SetActive(true);
                if (CorrectPipe)
                {
                    if (gm1 == CorrectPipe)
                    {
                        RightPipe = true;
                    }
                    else
                    {
                        RightPipe = false;
                    }
                }
                
            }
            if (Counter == 1)
            {
                gm1.SetActive(false);
                gm2.SetActive(true);
                if (CorrectPipe)
                {
                    if (gm2 == CorrectPipe)
                    {
                        RightPipe = true;
                    }
                    else
                    {
                        RightPipe = false;
                    }
                }
            }
            if (Counter == 2)
            {
                gm2.SetActive(false);
                gm3.SetActive(true);
                if (CorrectPipe)
                {
                    if (gm3 == CorrectPipe)
                    {
                        RightPipe = true;
                    }
                    else
                    {
                        RightPipe = false;
                    }
                }
            }
            
            
        }
    }
}

