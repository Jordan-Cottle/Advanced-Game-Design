using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{

    public float MaxHealth;
    public Slider HealthBar;

    private float _currentHealth;
    public float CurrentHealth
    {
        get => _currentHealth; private set
        {
            _currentHealth = Mathf.Max(value, 0);
            HealthBar.value = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        HealthBar.minValue = 0;
        HealthBar.maxValue = MaxHealth;

        CurrentHealth = MaxHealth;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            CurrentHealth -= 5;
        }
    }
}
