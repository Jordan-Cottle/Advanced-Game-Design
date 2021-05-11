using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float ProjectileSpeed = 10f;
    public float Lifetime = 10f;
    public float BaseEnergyCost = 0.5f;

    public float Power = 1;

    void Start()
    {
        this.GetComponent<Rigidbody>().velocity = this.transform.forward * ProjectileSpeed;

        Destroy(this.gameObject, Lifetime);
    }
}














