using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{

    public float MaxHealth;
    public float RegenRate = 2f;
    public float RegenDelay = 5f;
    private float lastHit = 0;

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

    void Update()
    {
        if (Time.time - lastHit > RegenDelay)
        {
            CurrentHealth += RegenRate * Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            CurrentHealth -= 5;
            lastHit = Time.time;
        }
    }
}
