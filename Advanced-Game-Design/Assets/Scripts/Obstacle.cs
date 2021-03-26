using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int Durability = 5;
    public bool Active = true;

    private Vector3 gone = new Vector3(-100, -100, 0);

    void Update()
    {
        if (this.transform.position.z < -5)
        {
            this.Remove();
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        Durability -= 1;
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
