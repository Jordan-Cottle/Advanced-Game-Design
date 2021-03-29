using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class GoalManager : MonoBehaviour
{
    public Slider EscapeDistanceUI;
    public Slider EscapeVelocityUI;

    public Text WinLabel;

    private GameManager gameManager;

    private float distanceTraveled { get => gameManager.DistanceTraveled; }
    private float currentSpeed { get => gameManager.CurrentSpeed; }
    private float threatDistance { get => distanceTraveled - threatPosition; }

    public float ThreatSpeed = 2f;
    private float threatPosition = 0;

    public float EscapeDistance = 1000f;
    public float EscapeVelocity;

    private bool winSet = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GameManager>();

        EscapeDistanceUI.minValue = 0;
        EscapeDistanceUI.maxValue = EscapeDistance;

        EscapeVelocityUI.minValue = 0;
        EscapeVelocityUI.maxValue = EscapeVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        EscapeVelocityUI.value = currentSpeed;
        EscapeDistanceUI.value = threatDistance;

        int conditions_met = 0;

        if (threatDistance > EscapeDistance)
        {
            conditions_met += 1;
        }

        if (currentSpeed > EscapeVelocity)
        {
            conditions_met += 1;
        }

        threatPosition += ThreatSpeed * Time.deltaTime;

        if (conditions_met >= 2)
        {
            StartCoroutine(WinGame());
        }
    }

    IEnumerator WinGame()
    {
        if (winSet)
        {
            yield break;
        }

        WinLabel.text = "Congratulations, you win!";
        WinLabel.gameObject.SetActive(true);
        winSet = true;

        yield return new WaitForSeconds(5);

        Application.Quit();
    }
}
