using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class BackgroundController : MonoBehaviour
{

    [SerializeField]
    private Obstacle ObstaclePrefeb;
    [SerializeField]
    private Bullet BulletPrefab;

    [SerializeField]
    private float BulletSpawnRate;
    [SerializeField]
    private float ObstacleSpawnRate;
    [SerializeField]
    private float SpawnDistance;

    private Plane SpawnPlane;

    new private Camera camera;

    void Awake()
    {
        camera = GetComponent<Camera>();
        SpawnPlane = new Plane(transform.forward, SpawnDistance);
    }

    void Start()
    {
        StartCoroutine(SpawnBullets());
        StartCoroutine(SpawnObstacles());
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            Vector3 position = RandomInScreenPosition();

            Obstacle obstacle = Instantiate(
                ObstaclePrefeb,
                position,
                Random.rotation
            );

            float scale = Random.Range(Obstacle.MinScale, Obstacle.MaxScale);
            float density = Random.Range(Obstacle.MinDensity, Obstacle.MaxDensity);
            Vector3 angularVelocity = new Vector3(Random.value, Random.value, Random.value);
            Vector3 startKick = Random.insideUnitSphere * 3;

            obstacle.Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
            obstacle.Setup(
                scale,
                density,
                angularVelocity,
                startKick
            );

            yield return new WaitForSeconds(ObstacleSpawnRate);
        }
    }

    IEnumerator SpawnBullets()
    {
        while (true)
        {
            Vector3 position = RandomOffScreenPosition();
            Vector3 screenCenter = ScreenPositionToSpawnPlane(
                new Vector3(
                    camera.pixelWidth / 2,
                    camera.pixelHeight / 2,
                    0
                )
            );

            Bullet bullet = Instantiate(
                BulletPrefab,
                position,
                Quaternion.LookRotation(
                    screenCenter - position
                )
            );


            bullet.Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;

            yield return new WaitForSeconds(BulletSpawnRate);
        }
    }

    Vector3 ScreenPositionToSpawnPlane(Vector3 screenPosition)
    {
        Ray ray = camera.ScreenPointToRay(screenPosition);

        float distance;
        bool hit = SpawnPlane.Raycast(ray, out distance);

        Vector3 position = (ray.direction - ray.origin) * -distance;
        position.z = SpawnDistance;
        return position;
    }

    Vector3 RandomInScreenPosition()
    {
        Vector3 screenPosition = new Vector3(
            Random.Range(0, camera.pixelWidth),
            Random.Range(0, camera.pixelHeight),
            0
        );

        return ScreenPositionToSpawnPlane(screenPosition);
    }

    Vector3 RandomOffScreenPosition()
    {
        Vector3 planeCenter = new Vector3(0, 0, SpawnDistance);
        Vector3 bottomLeft = ScreenPositionToSpawnPlane(Vector3.zero);
        float radius = (planeCenter - bottomLeft).magnitude * 1.25f;
        Debug.Log($"Radius: {radius}");

        float angle = Random.Range(0, Mathf.PI * 2f);
        Vector3 position = new Vector3(
            Mathf.Cos(angle),
            Mathf.Sin(angle),
            0
        ) * radius;

        position.z = SpawnDistance;

        return position;
    }
}
