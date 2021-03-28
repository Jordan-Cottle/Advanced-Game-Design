using UnityEngine;
using UnityEngine.UI;


public class GoalManager : MonoBehaviour
{
    public Slider EscapeDistanceUI;
    public Slider EscapeVelocityUI;

    private GameManager gameManager;

    private float distanceTraveled { get => gameManager.DistanceTraveled; }
    private float currentSpeed { get => gameManager.CurrentSpeed; }
    private float threatDistance { get => distanceTraveled - threatPosition; }

    public float ThreatSpeed = 2f;
    private float threatPosition = 0;

    public float EscapeDistance = 1000f;
    public float EscapeVelocity;

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

        if (threatDistance > EscapeDistance)
        {
            Debug.Log("Escape distance achieved!");
        }
        else
        {
            Debug.Log($"Threat is {threatDistance} away!");
        }

        if (currentSpeed > EscapeVelocity)
        {
            Debug.Log("Escape velocity Achieved!");
        }
        else
        {
            Debug.Log($"Still need to accelerate by {EscapeVelocity - (currentSpeed)}");
        }

        threatPosition += ThreatSpeed * Time.deltaTime;
    }
}
