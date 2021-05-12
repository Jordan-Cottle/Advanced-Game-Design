using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : CapacityContainer
{

    public float RechargeRate;
    public float RechargeDelay;

    private float _timeSinceLastEvent;

    public Text LoseLabel;

    private ParticleSystem deathExplosion;

    void Awake()
    {
        deathExplosion = transform.Find("BigExplosion").GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (_timeSinceLastEvent > RechargeDelay)
        {
            PassiveRecharge();
        }

        _timeSinceLastEvent += Time.deltaTime;
    }

    void PassiveRecharge()
    {
        CurrentCapacity = CurrentCapacity + (RechargeRate * Time.deltaTime);
    }

    IEnumerator LoseGame()
    {
        LoseLabel.text = "Oops, looks like you lost!";
        LoseLabel.color = Color.red;
        LoseLabel.gameObject.SetActive(true);

        AudioManager.Instance?.Play("Explode");
        deathExplosion.Play(true);

        yield return new WaitForSeconds(5);

        SceneManager.LoadScene("MainMenu");
        Cursor.lockState = CursorLockMode.None;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            if (CurrentCapacity <= 0)
            {
                StartCoroutine(LoseGame());
                return;
            }

            Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
            CurrentCapacity -= collision.relativeVelocity.magnitude * collision.rigidbody.mass;
            AudioManager.Instance.Play("ShieldHit");
            // TODO: create particles for player damaged
            _timeSinceLastEvent = 0;
        }
    }
}
