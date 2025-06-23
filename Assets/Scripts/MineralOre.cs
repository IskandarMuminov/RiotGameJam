using UnityEngine;
using Random = UnityEngine.Random;

public class MineralOre : MonoBehaviour
{
    [SerializeField] private GameObject mineralOre;
    [SerializeField] float spawnRadius = 5f;
    private int mineralToSpawn;

    private void Start()
    {
        mineralToSpawn = Random.Range(2, 5);
    }
    private void OnDestroy()
    {
        for (int i = 0; i < mineralToSpawn; i++)
        {
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection.y = 0;
        Vector3 spawnPosition = transform.position + randomDirection;
        Instantiate(mineralOre, spawnPosition, Quaternion.identity);
    }
}