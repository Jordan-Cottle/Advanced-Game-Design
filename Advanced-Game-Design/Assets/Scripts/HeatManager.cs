using UnityEngine;
using System.Collections;

public class HeatManager : CapacityContainer
{
    [SerializeField]
    private float _cooldownRate;
    public float CooldownRate { get => _cooldownRate / 100f; }
    public float CooldownDelay;

    private float _timeSinceLastEvent;

    public bool Overheated { get; private set; }

    // Start is called before the first frame update
    new void Start()
    {
        CurrentCapacity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (CooldownDelay > 0 || _timeSinceLastEvent > CooldownDelay)
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

        if (Full)
        {
            ForceCooldown();
        }
    }

    void ForceCooldown()
    {
        Debug.Log("Too much heat, forced cooldown activating");
        Disable();
        StartCoroutine(EnableAfter(2f));
    }
    void Disable()
    {
        // TODO: play overheat sound
        // TODO: display overheat visuals
        Overheated = true;
    }

    IEnumerator EnableAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        Overheated = false;
    }
}
