using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public GameObject Pipe1;
    public GameObject Pipe2;
    public GameObject Pipe3;

    [SerializeField] private GameObject Player;
        
    [SerializeField] GameObject Water;
    // Update is called once per frame

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (Pipe1.GetComponent<TurningPipe>().RightPipe == true && Pipe2.GetComponent<TurningPipe>().RightPipe == true && Pipe3.GetComponent<TurningPipe>().RightPipe && true)
        {
            if (Player.GetComponent<ResourceManager>())
            {
                Player.GetComponent<ResourceManager>().IncreaseWater();
                if (Water)
                {
                    Water.SetActive(true);
                }
            }
            
            Destroy(this.gameObject);
        }
    }
}
