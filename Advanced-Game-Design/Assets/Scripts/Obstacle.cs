using UnityEngine;

public class Obstacle : MonoBehaviour
{

    public static readonly float MinScale = 1;
    public static readonly float MaxScale = 5;
    public static readonly float MinDensity = 0.5f;
    public static readonly float MaxDensity = 2;

    private static readonly float DensityRange = MaxDensity - MinDensity;

    private static readonly float MinDurability = MinScale * MinDensity;
    private static readonly float MaxDurability = MaxScale * MaxDensity;
    private static readonly float DurabilityRange = MaxDurability - MinDurability;

    public float Density = 1;
    public bool Active = true;

    private float Durability = 5;
    private float StartDurability;

    private static Vector3 gone = new Vector3(-100, -100, 0);
    new private Rigidbody rigidbody;
    new private Renderer renderer;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
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
        float colorScale = (Density - MinDensity) / DensityRange;
        // Less durability == more transparent TODO: use texture instead
        float alphaScale = Mathf.Clamp(0.25f + (Durability / StartDurability), 0.25f, 1.0f);
        Color color = new Color(colorScale, colorScale, colorScale, alphaScale);
        renderer.material.SetColor("_Color", color);
    }

    void Update()
    {
        if (this.transform.position.z < -5)
        {
            this.Remove();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            this.Remove();
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
            this.Remove();
        }
    }

    void Remove()
    {
        this.transform.position = gone;
        this.Active = false;
    }
}
