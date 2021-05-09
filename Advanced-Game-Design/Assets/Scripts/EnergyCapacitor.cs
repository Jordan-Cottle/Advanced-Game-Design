using UnityEngine;

public class NotEnoughEnergyError : System.Exception
{
    public NotEnoughEnergyError(string message) : base(message) { }
}
public class EnergyCapacitor : CapacityContainer
{
    public float RechargeRate;
    public float RechargeDelay;

    private float _timeSinceLastEvent;

    // Update is called once per frame
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

    public void UseEnergy(float amount, bool triggerRechargeCooldown = true)
    {
        if (amount > CurrentCapacity)
        {
            throw new NotEnoughEnergyError($"{gameObject} tried to spend {amount} energy but only has {CurrentCapacity}");
        }

        CurrentCapacity -= amount;
        if (triggerRechargeCooldown)
        {
            _timeSinceLastEvent = 0;
        }

        Debug.Log($"{gameObject} spent {amount} energy and has {CurrentCapacity} remaining");
    }
}
