using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Obstacle obstaclePrefab;
    public float Acceleration = 5f;
    public float MaxSpeed = 25f;

    public List<Obstacle> Obstacles;
    public List<GameObject> TunnelPieces;

    private Vector3 tunnelJump;
    private float tunnelScale;

    Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(0, 0, 0);
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

        foreach (var obstacle in Obstacles)
        {
            Rigidbody body = obstacle.GetComponent<Rigidbody>();
            body.velocity = velocity;
        }

        Vector3 offset = velocity * Time.deltaTime;
        foreach (var tunnelPiece in TunnelPieces)
        {
            tunnelPiece.transform.Translate(offset, Space.World);

            if (tunnelPiece.transform.position.z < -tunnelScale)
            {
                tunnelPiece.transform.Translate(tunnelJump, Space.World);
            }
        }
    }
}
