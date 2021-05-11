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
        CurrentCapacity = Mathf.Min(CurrentCapacity + (RechargeRate * Time.deltaTime), MaxCapacity);
    }

    IEnumerator LoseGame()
    {
        LoseLabel.text = "Oops, looks like you lost!";
        LoseLabel.color = Color.red;
        LoseLabel.gameObject.SetActive(true);

        // TODO: Play sound player destroyed
        // TODO: Play animation for player destroyed

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
            }
            CurrentCapacity -= collision.relativeVelocity.magnitude;
            // TODO: play sound player damaged
            // TODO: create particles for player damaged
            _timeSinceLastEvent = 0;
        }
    }
}
