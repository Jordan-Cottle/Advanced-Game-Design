using UnityEngine;

public class HeatManager : CapacityContainer
{
    [SerializeField]
    private float _cooldownRate;
    public float CooldownRate { get => _cooldownRate / 100f; }
    public float CooldownDelay;

    private float _timeSinceLastEvent;

    // Start is called before the first frame update
    new void Start()
    {
        CurrentCapacity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timeSinceLastEvent > CooldownDelay)
        {
            Cooldown();
        }

        _timeSinceLastEvent += Time.deltaTime;
    }

    void Cooldown()
    {
        CurrentCapacity -= (CooldownRate * CurrentCapacity) * Time.deltaTime;
        Debug.Log($"Ship cooling: {CurrentCapacity}");
    }

    public void GenerateHeat(float amount, bool resetCooldown = true)
    {
        CurrentCapacity += amount;

        if (resetCooldown)
        {
            _timeSinceLastEvent = 0;
        }
    }
}
