using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public delegate void ObstacleCollision(float obstacleMass, Vector3 collisionVelocity);
    public static event ObstacleCollision PlayerHit;

    private ScoreManager scoreManager;

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

    new private Rigidbody rigidbody;
    new private Renderer renderer;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
        scoreManager = FindObjectOfType<ScoreManager>();
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
        // Denser material  == Darker
        float colorScale = (MaxDensity - Density) / DensityRange;
        // Less durability == more transparent TODO: use texture instead
        float alphaScale = Mathf.Clamp(0.25f + (Durability / StartDurability), 0.25f, 1.0f);
        Color color = new Color(colorScale, colorScale, colorScale, alphaScale);
        renderer.material.SetColor("_Color", color);
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
