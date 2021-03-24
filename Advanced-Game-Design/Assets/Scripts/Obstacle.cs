using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int Durability = 5;
    public bool Active = true;

    void Update()
    {
        if (this.transform.position.z < -5)
        {
            float x;
            float y;
            float z;
            if (this.Active)
            {
                x = Random.Range(-10f, 10f);
                y = Random.Range(-10f, 10f);
                z = Random.Range(25f, 50f);
            }
            else
            {
                x = -100;
                y = -100;
                z = 0;
            }

            this.transform.position = new Vector3(x, y, z);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        Durability -= 1;
        if (Durability <= 0)
        {
            Debug.Log("Obstacle destroyed!");
            this.transform.position = new Vector3(-100, -100, 0);
            this.Active = false;
        }
    }
}
