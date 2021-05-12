using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour
{
    public delegate void ObstacleCollision(float obstacleMass, Vector3 collisionVelocity);
    public static event ObstacleCollision PlayerHit;

    private ScoreManager scoreManager;
    private ParticleSystem collisionRubble;
    private ParticleSystem finalExplosion;

    public static readonly float MinScale = 1;
    public static readonly float MaxScale = 5;
    public static readonly float MinDensity = 0.5f;
    public static readonly float MaxDensity = 2;

    private static readonly float DensityRange = MaxDensity - MinDensity;

    private static readonly float MinDurability = MinScale * MinDensity;
    private static readonly float MaxDurability = MaxScale * MaxDensity;
    private static readonly float DurabilityRange = MaxDurability - MinDurability;

    private float _density;
    public float Density
    {
        get => _density;
        set
        {
            _density = value;

            Rigidbody.mass = transform.localScale.magnitude * Density;
            StartDurability = Rigidbody.mass * Density;
            Durability = StartDurability;
            Recolor();
        }
    }
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
        get => Rigidbody.velocity;
        set
        {
            var otherVelocity = Rigidbody.velocity - _velocity;
            _velocity = value;
            Rigidbody.velocity = otherVelocity + _velocity;
        }
    }

    public Rigidbody Rigidbody { get; private set; }
    new private Renderer renderer;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
        scoreManager = FindObjectOfType<ScoreManager>();
        collisionRubble = transform.Find("CollisionRubble").GetComponent<ParticleSystem>();
        finalExplosion = transform.Find("EnergyExplosion").GetComponent<ParticleSystem>();
    }

    public void Setup(float scale, float density, Vector3 angularVelocity)
    {
        Setup(
            new Vector3(scale, scale, scale),
            density,
            angularVelocity,
            Vector3.zero
        );
    }
    public void Setup(float scale, float density, Vector3 angularVelocity, Vector3 startKick)
    {
        Setup(
            new Vector3(scale, scale, scale),
            density,
            angularVelocity,
            startKick
        );
    }
    public void Setup(Vector3 scale, float density, Vector3 angularVelocity, Vector3 startKick)
    {
        transform.localScale = scale;
        Density = density;
        Active = true;
        Rigidbody.angularVelocity = angularVelocity;
        Rigidbody.AddForce(startKick, ForceMode.Impulse);
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
            PlayerHit?.Invoke(Rigidbody.mass, collision.relativeVelocity);
            this.Deactivate();
            return;
        }

        if (collision.gameObject.tag == "Bullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            Durability -= bullet.Power;
            Recolor();

            collisionRubble.transform.rotation = Quaternion.LookRotation(collision.gameObject.transform.position - transform.position);
            collisionRubble.Emit((int)Mathf.Ceil(collision.relativeVelocity.magnitude));

            AudioManager.Instance?.Play("Hit");
            scoreManager?.AddScore(1);
        }

        if (Durability <= 0)
        {
            scoreManager?.AddScore(StartDurability);

            collisionRubble.transform.rotation = Quaternion.LookRotation(transform.position - collision.gameObject.transform.position);
            Explode();
        }
    }

    void Explode()
    {
        collisionRubble.transform.parent = null;
        collisionRubble.Emit((int)(transform.localScale.magnitude * 10));
        finalExplosion.transform.parent = null;
        finalExplosion.Play(true);
        StartCoroutine(cleanupExplosionAfter(finalExplosion.main.duration));
        Deactivate();
    }

    IEnumerator cleanupExplosionAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        finalExplosion.Clear(true);
        finalExplosion.transform.parent = transform;
        collisionRubble.transform.parent = transform;
    }

    void Stop()
    {
        Rigidbody.velocity = Vector3.zero;
        _velocity = Vector3.zero;
    }

    public void Deactivate()
    {
        Stop();
        this.Active = false;
    }
}
