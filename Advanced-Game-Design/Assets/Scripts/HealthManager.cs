using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{

    public float MaxHealth;
    public float RegenRate = 2f;
    public float RegenDelay = 5f;
    private float lastHit = 0;

    public Slider HealthBar;
    public Text LoseLabel;

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

    IEnumerator LoseGame()
    {
        LoseLabel.text = "Oops, looks like you lost!";
        LoseLabel.color = Color.red;
        LoseLabel.gameObject.SetActive(true);

        yield return new WaitForSeconds(5);

        SceneManager.LoadScene("MainMenu");
        Cursor.lockState = CursorLockMode.None;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            if (CurrentHealth <= 0)
            {
                StartCoroutine(LoseGame());
            }
            CurrentHealth -= 5;
            lastHit = Time.time;
        }
    }
}
