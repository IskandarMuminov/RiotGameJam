using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightbeamSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] lightbeams;
    [SerializeField] private float spawnRate;
    [SerializeField] private bool isInCave;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInCave = true;
            //function to switch the camera view
            StartCoroutine(SpawnLightbeams());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInCave = false;
        }
    }

    public IEnumerator SpawnLightbeams() {
        while (isInCave)
        {
            for (int i = 0; i < lightbeams.Length; i++)
            {
                i = Random.Range(0, lightbeams.Length);

                lightbeams[i].SetActive(true);
                yield return new WaitForSecondsRealtime(Random.Range(spawnRate, spawnRate + 1));
                lightbeams[i].SetActive(false);
            }

        }

    }

}
