using UnityEngine;

public class MineralConsumable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        ResourceManager.Instance.IncreaseMineral();
    }
}
