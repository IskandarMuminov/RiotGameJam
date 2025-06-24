using System;
using Microlight.MicroBar;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class MineralOre : MonoBehaviour
{
    [SerializeField] private GameObject mineralOre;
    [SerializeField] float spawnRadius = 5f;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject QteBar;
    
    private int mineralToSpawn;

    void OnInteractPressed()
    {
        throw new NotImplementedException();
    }

    private void OnDestroy()
    {
        SpawnObject();
    }

    void SpawnObject()
    {
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection.y = 0;
        Vector3 spawnPosition = transform.position + randomDirection;
        Instantiate(mineralOre, spawnPosition, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            healthBar.SetActive(true);
            //QteBar.SetActive(true);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            QteBar.SetActive(true);
        }
    }
}