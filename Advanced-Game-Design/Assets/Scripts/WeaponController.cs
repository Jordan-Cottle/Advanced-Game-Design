using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{

    public Bullet Projectile;

    public EnergyCapacitor EnergySource;

    public float AutoFireRate = 0.125f;

    private float timeSinceLastFire = 0;

    private HeatManager heatManager;
    private float heatTax => 1 + (heatManager.CurrentCapacity / 100);

    void Awake()
    {
        heatManager = GetComponentInParent<HeatManager>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastFire += Time.deltaTime;
        if (Input.GetMouseButton(0))
        {
            RapidFire();
        }
    }

    void RapidFire()
    {
        if (heatManager.Overheated)
        {
            // TODO: play failed to fire sound
            return;
        }

        // auto-fire delay can be overridden by spamming the fire button
        if (timeSinceLastFire < AutoFireRate && !Input.GetMouseButtonDown(0))
        {
            return;
        }

        float energyCost = Projectile.BaseEnergyCost * heatTax;
        if (EnergySource.CurrentCapacity > energyCost)
        {
            EnergySource.UseEnergy(energyCost);
            Bullet shot = Instantiate(Projectile, this.transform.position + this.transform.forward * 1, this.transform.rotation);
            timeSinceLastFire = 0;
            heatManager.GenerateHeat(15);
            AudioManager.Instance?.Play("Shoot");
        }
    }
}
