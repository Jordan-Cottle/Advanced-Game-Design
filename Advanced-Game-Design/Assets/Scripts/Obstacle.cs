using UnityEngine;

public class Obstacle : MonoBehaviour
{

    public static readonly float MinScale = 1;
    public static readonly float MaxScale = 5;
    public static readonly float MinDensity = 0.5f;
    public static readonly float MaxDensity = 2;

    private static readonly float DensityRange = MaxDensity - MinDensity;

    public float Density = 1;
    public bool Active = true;

    private float Durability = 5;

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

        Durability = rigidbody.mass * Density;

        float colorScale = (Density - MinDensity) / DensityRange; // Denser materials  == Darker
        Color color = new Color(colorScale, colorScale, colorScale);
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
