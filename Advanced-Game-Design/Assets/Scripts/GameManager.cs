using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Obstacle obstaclePrefab;

    [SerializeField]
    private List<Texture> _tunnelTextures;
    public float StartSpeed = 5f;
    public float Acceleration = 0.5f;
    public float MaxSpeed = 25f;
    public float DistanceTraveled { get; private set; }

    public float RampUpInterval;

    public static float SpawnDistance = 150;
    public float ObstacleStartingForce;

    private Queue<Obstacle> deactivatedObstacles = new Queue<Obstacle>();
    private Queue<Obstacle> availableObstacles = new Queue<Obstacle>();
    private List<Obstacle> obstacles = new List<Obstacle>();
    public List<GameObject> TunnelPieces;
    public Text SpeedLabel;

    private Vector3 tunnelJump;
    private float tunnelScale;

    private Vector3 velocity;
    public float CurrentSpeed { get => -velocity.z; }

    private float _nextSpawnMilestone = 0;

    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(0, 0, -StartSpeed);

        tunnelScale = TunnelPieces[0].transform.localScale.y;
        tunnelJump = new Vector3(0, 0, tunnelScale * 4);

        StartCoroutine("Spawn");

        Cursor.lockState = CursorLockMode.Locked;

        Texture texture = _tunnelTextures[Random.Range(0, _tunnelTextures.Count - 1)];
        foreach (var tunnelPiece in TunnelPieces)
        {
            var renderer = tunnelPiece.GetComponent<Renderer>();

            renderer.material.SetTexture("_MainTex", texture);
        }
    }

    void OnEnable()
    {
        Obstacle.PlayerHit += PlayerCollisionHandler;
    }

    void OnDisable()
    {
        Obstacle.PlayerHit -= PlayerCollisionHandler;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
            Cursor.lockState = CursorLockMode.None;
        }

        Accelerate(Acceleration * Time.deltaTime);

        foreach (var obstacle in obstacles)
        {
            if (obstacle.Active)
            {
                obstacle.Velocity = velocity;
            }
            else
            {
                deactivatedObstacles.Enqueue(obstacle);
            }
        }

        while (deactivatedObstacles.Count > 0)
        {
            Obstacle obstacle = deactivatedObstacles.Dequeue();
            obstacle.gameObject.SetActive(false);
            availableObstacles.Enqueue(obstacle);
            obstacles.Remove(obstacle);
        }

        Vector3 offset = velocity * Time.deltaTime;

        DistanceTraveled += -offset.z;
        if (DistanceTraveled >= _nextSpawnMilestone)
        {
            Spawn();
        }
        Debug.Log($"Distance traveled: {DistanceTraveled}");

        foreach (var tunnelPiece in TunnelPieces)
        {
            tunnelPiece.transform.Translate(offset, Space.World);

            if (tunnelPiece.transform.position.z < -tunnelScale)
            {
                tunnelPiece.transform.Translate(tunnelJump, Space.World);
            }
        }
    }

    private void Spawn()
    {
        float difficulty = DistanceTraveled / RampUpInterval;
        int obstaclesToSpawn = Random.Range(1, (int)(difficulty) + 3);
        for (int i = 0; i < obstaclesToSpawn; i++)
        {
            SpawnObstacle();
        }

        _nextSpawnMilestone += Random.Range(75, 125) / Mathf.Max(difficulty, 5) * 5;
    }

    private void SpawnObstacle()
    {
        Vector3 pos = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 150);
        Obstacle obstacle;
        if (availableObstacles.Count == 0)
        {
            obstacle = Instantiate(obstaclePrefab, pos, Quaternion.identity);
        }
        else
        {
            obstacle = availableObstacles.Dequeue();
            obstacle.transform.position = pos;
        }

        float scale = Random.Range(Obstacle.MinScale, Obstacle.MaxScale);
        float density = Random.Range(Obstacle.MinDensity, Obstacle.MaxDensity);
        Vector3 angularVelocity = new Vector3(Random.value, Random.value, Random.value);
        Vector3 startKick = Random.insideUnitSphere * ObstacleStartingForce;

        obstacle.Setup(
            scale,
            density,
            angularVelocity,
            startKick
        );

        obstacles.Add(obstacle);
    }

    public void Accelerate(float amount)
    {
        velocity.z = Mathf.Clamp(velocity.z - amount, -MaxSpeed, 0f);
        SpeedLabel.text = $"Speed: {CurrentSpeed}";
    }

    public void PlayerCollisionHandler(float obstacleMass, Vector3 collisionVelocity)
    {
        // Slow down the player when they hit an obstacle
        float velocityCost = Mathf.Min(obstacleMass * collisionVelocity.magnitude, velocity.magnitude / 2);

        Accelerate(-velocityCost);
    }
}
