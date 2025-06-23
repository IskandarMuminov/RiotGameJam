using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightbeamSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] lightbeams;
    [SerializeField] private float spawnRate;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //function to switch the camera view
            StartCoroutine(SpawnLightbeams());
        }
    }

    public IEnumerator SpawnLightbeams() {

        for (int i = 0; i < lightbeams.Length; i++)
        {
            lightbeams[i].SetActive(true);
            yield return new WaitForSecondsRealtime(Random.Range(0, spawnRate));
            lightbeams[i].SetActive(false);
        }

    }

}
