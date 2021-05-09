using UnityEngine;

public class WeaponController : MonoBehaviour
{

    public Bullet Projectile;

    public EnergyCapacitor EnergySource;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Fire!");
            if (EnergySource.CurrentCapacity > Projectile.FireEnergyCost)
            {
                EnergySource.UseEnergy(Projectile.FireEnergyCost);
                Bullet shot = Instantiate(Projectile, this.transform.position + this.transform.forward * 1, this.transform.rotation);
            }
        }
    }
}
