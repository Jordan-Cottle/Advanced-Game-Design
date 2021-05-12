using UnityEngine;
using System.Collections.Generic;

public class Obstacle : MonoBehaviour
{
    public delegate void ObstacleCollision(float obstacleMass, Vector3 collisionVelocity);
    public static event ObstacleCollision PlayerHit;

    private ScoreManager scoreManager;
    new private ParticleSystem particleSystem;

    public static readonly float MinScale = 1;
    public static readonly float MaxScale = 5;
    public static readonly float MinDensity = 0.5f;
    public static readonly float MaxDensity = 2;

    private static readonly float DensityRange = MaxDensity - MinDensity;

    private static readonly float MinDurability = MinScale * MinDensity;
    private static readonly float MaxDurability = MaxScale * MaxDensity;
    private static readonly float DurabilityRange = MaxDurability - MinDurability;

    public float Density = 1;
    public bool Active
    {
        get => gameObject.activeSelf;
        set => gameObject.SetActive(value);
    }

    private float Durability = 5;
    private float StartDurability;

    private Vector3 _velocity;
    public Vector3 Velocity
    {
        get => rigidbody.velocity;
        set
        {
            var otherVelocity = rigidbody.velocity - _velocity;
            _velocity = value;
            rigidbody.velocity = otherVelocity + _velocity;
        }
    }

    new public Rigidbody rigidbody { get; private set; }
    new private Renderer renderer;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
        scoreManager = FindObjectOfType<ScoreManager>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    void OnEnable()
    {
        rigidbody.mass = transform.localScale.magnitude * Density;

        StartDurability = rigidbody.mass * Density;
        Durability = StartDurability;

        Recolor();
    }

    void Recolor()
    {
        float densityRatio = Density / MaxDensity;

        renderer.material.SetFloat("_DensityRatio", densityRatio);
        renderer.material.SetFloat("_MaxDurability", StartDurability);
        renderer.material.SetFloat("_Durability", Durability);
    }

    void Update()
    {
        if (this.transform.position.z < -5)
        {
            this.Deactivate();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHit?.Invoke(rigidbody.mass, collision.relativeVelocity);
            this.Deactivate();
            return;
        }

        if (collision.gameObject.tag == "Bullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            Durability -= bullet.Power;
            Recolor();

            particleSystem.transform.rotation = Quaternion.LookRotation(collision.gameObject.transform.position - transform.position);
            particleSystem.Emit((int)Mathf.Ceil(collision.relativeVelocity.magnitude));


            AudioManager.Instance?.Play("Hit");

            Debug.Log($"Obstacle hit: {Durability} durability remaining");
        }

        if (Durability <= 0)
        {
            Debug.Log("Obstacle destroyed!");
            scoreManager.AddScore(StartDurability);
            this.Deactivate();
        }
    }

    void Stop()
    {
        rigidbody.velocity = Vector3.zero;
        _velocity = Vector3.zero;
    }

    public void Deactivate()
    {
        Stop();
        this.Active = false;
    }
}
