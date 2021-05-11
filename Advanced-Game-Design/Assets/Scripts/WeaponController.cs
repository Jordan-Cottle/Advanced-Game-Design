using UnityEngine;

public class WeaponController : MonoBehaviour
{

    public Bullet Projectile;

    public EnergyCapacitor EnergySource;

    public float AutoFireRate = 0.125f;

    private float timeSinceLastFire = 0;

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
        // auto-fire delay can be overridden by spamming the fire button
        if (timeSinceLastFire < AutoFireRate && !Input.GetMouseButtonDown(0))
        {
            return;
        }

        if (EnergySource.CurrentCapacity > Projectile.RapidFireEnergyCost)
        {
            EnergySource.UseEnergy(Projectile.RapidFireEnergyCost);
            Bullet shot = Instantiate(Projectile, this.transform.position + this.transform.forward * 1, this.transform.rotation);
            timeSinceLastFire = 0;
        }
    }
}
