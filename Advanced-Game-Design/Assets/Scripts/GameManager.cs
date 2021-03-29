using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Obstacle obstaclePrefab;
    public float StartSpeed = 5f;
    public float Acceleration = 0.5f;
    public float MaxSpeed = 25f;

    private float _distanceTraveled = 0;
    public float DistanceTraveled { get => _distanceTraveled; private set => _distanceTraveled = value; }

    public static float SpawnDistance = 150;

    private Queue<Obstacle> deactivatedObstacles = new Queue<Obstacle>();
    private Queue<Obstacle> availableObstacles = new Queue<Obstacle>();
    private List<Obstacle> obstacles = new List<Obstacle>();
    public List<GameObject> TunnelPieces;
    public Text SpeedLabel;

    private Vector3 tunnelJump;
    private float tunnelScale;

    private Vector3 velocity;
    public float CurrentSpeed { get => -velocity.z; }

    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(0, 0, -StartSpeed);
        tunnelJump = new Vector3(0, 0, 0);

        tunnelScale = TunnelPieces[0].transform.localScale.y;
        tunnelJump.z = tunnelScale * 4;

        StartCoroutine("Spawn");

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        velocity.z = Mathf.Clamp(velocity.z - Acceleration * Time.deltaTime, -MaxSpeed, 0f);
        SpeedLabel.text = $"Speed: {CurrentSpeed}";

        foreach (var obstacle in obstacles)
        {
            Rigidbody body = obstacle.GetComponent<Rigidbody>();
            if (obstacle.Active)
            {
                body.velocity = velocity;
            }
            else
            {
                body.velocity = Vector3.zero;
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

        foreach (var tunnelPiece in TunnelPieces)
        {
            tunnelPiece.transform.Translate(offset, Space.World);

            if (tunnelPiece.transform.position.z < -tunnelScale)
            {
                tunnelPiece.transform.Translate(tunnelJump, Space.World);
            }
        }
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            float difficultyRatio = CurrentSpeed / MaxSpeed;
            for (int i = 0; i < Random.Range(1, (int)(15 * difficultyRatio) + 3); i++)
            {
                SpawnObstacle();
            }
            float spawnDelay = Mathf.Max(0.1f, 0.5f - difficultyRatio);
            yield return new WaitForSeconds(spawnDelay);
        }
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

        obstacle.gameObject.SetActive(true);

        float scale = Random.Range(1f, 3f);
        obstacle.transform.localScale = new Vector3(scale, scale, scale);

        Rigidbody body = obstacle.GetComponent<Rigidbody>();
        body.mass = scale * 10f;
        body.angularVelocity = new Vector3(Random.value, Random.value, Random.value);
        obstacle.Durability = (int)Mathf.Round(scale);
        obstacle.Active = true;

        obstacle.GetComponent<Renderer>().material.SetColor("_Color", Random.ColorHSV());

        obstacles.Add(obstacle);
    }
}
