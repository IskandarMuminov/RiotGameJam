using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public GameObject Pipe1;
    public GameObject Pipe2;
    public GameObject Pipe3;

    // Update is called once per frame
    void Update()
    {
        if (Pipe1.GetComponent<TurningPipe>().RightPipe == true || Pipe2.GetComponent<TurningPipe>().RightPipe == true || Pipe3.GetComponent<TurningPipe>().RightPipe == true)
        {
            Destroy(this.gameObject);
        }
    }
}
