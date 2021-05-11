using UnityEngine;

public class MovementController : MonoBehaviour
{
    // Start is called before the first frame update
    public float MovementSpeed = 15f;

    [SerializeField]
    private float brakingPower;
    [SerializeField]
    private float brakingEnergyCost;
    [SerializeField]
    private float brakingHeatCost;

    [SerializeField]
    private float boostPower;
    [SerializeField]
    private float boostEnergyCost;
    [SerializeField]
    private float boostingHeatCost;

    public CharacterController Controller;

    private EnergyCapacitor energyCapacitor;
    private HeatManager heatManager;
    private GameManager gameManager;

    private Vector3 _velocity;
    public Vector3 Velocity { get => _velocity; }

    private bool boosting = false;
    private bool braking = false;


    void Awake()
    {
        energyCapacitor = GetComponent<EnergyCapacitor>();
        heatManager = GetComponent<HeatManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal") * MovementSpeed;
        float verticalInput = Input.GetAxis("Vertical") * MovementSpeed;

        _velocity.x = horizontalInput;
        _velocity.y = verticalInput;

        Controller.Move(Velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            braking = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            braking = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            boosting = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            boosting = false;
        }

        if (braking)
        {
            SlowDown();
        }
        else if (boosting)
        {
            SpeedUp();
        }
    }

    void SlowDown()
    {
        if (heatManager.Overheated)
        {
            Debug.Log("Overheated, cannot slow down");
            // TODO: play too hot sound
            return;
        }
        else if (energyCapacitor.Empty)
        {
            Debug.Log("Not enough energy to slow down");
            // TODO: play no energy sound
            return;
        }

        heatManager.GenerateHeat(brakingHeatCost * Time.deltaTime);
        energyCapacitor.UseEnergy(brakingEnergyCost * Time.deltaTime);
        gameManager.Accelerate(-brakingPower * Time.deltaTime);
    }

    void SpeedUp()
    {
        if (heatManager.Overheated)
        {
            // TODO: play too hot sound
            Debug.Log("Overheated, cannot accelerate");
            return;
        }
        else if (energyCapacitor.Empty)
        {
            // TODO: play no energy sound
            Debug.Log("Not enough energy to accelerate");
            return;
        }

        heatManager.GenerateHeat(boostingHeatCost * Time.deltaTime);
        energyCapacitor.UseEnergy(boostEnergyCost * Time.deltaTime);
        gameManager.Accelerate(boostPower * Time.deltaTime);
    }
}
