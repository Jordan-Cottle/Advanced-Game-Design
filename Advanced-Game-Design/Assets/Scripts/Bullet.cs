using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public float ProjectileSpeed = 10f;
    public float Lifetime = 10f;
    public float BaseEnergyCost = 0.5f;

    public float Power = 1;

    private Rigidbody _rigidbody;
    public Rigidbody Rigidbody { get => _rigidbody; }

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Rigidbody.velocity = this.transform.forward * ProjectileSpeed;

        Destroy(this.gameObject, Lifetime);
    }
}














