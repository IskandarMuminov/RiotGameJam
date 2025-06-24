using Microlight.MicroBar;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private float _damage = 0;
    [SerializeField] private MicroBar healthBar;
    //[SerializeField] private MineralOre damageBar;

    private void Start()
    {
        healthBar.Initialize(_health);
        Debug.Log(healthBar.CurrentValue);
    }

    public void TakeDamage()
    {
        healthBar.UpdateBar(healthBar.CurrentValue - _damage);
        if (healthBar.CurrentValue <= 0)
        {
            Destroy(gameObject);
        }
    }
}
