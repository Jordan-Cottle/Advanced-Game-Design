using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float ProjectileSpeed = 10f;
    public float Lifetime = 10f;

    void Start()
    {
        this.GetComponent<Rigidbody>().velocity = this.transform.forward * ProjectileSpeed;

        Destroy(this.gameObject, Lifetime);
    }
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Bullet collided with something");

        if (other.gameObject.tag != "Tunnel")
        {
            other.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Random.ColorHSV());
        }
    }
}














