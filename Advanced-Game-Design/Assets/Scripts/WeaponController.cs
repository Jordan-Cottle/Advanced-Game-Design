using UnityEngine;

public class WeaponController : MonoBehaviour
{

    public Bullet projectile;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Fire!");
            Bullet shot = Instantiate(projectile, this.transform.position + this.transform.forward * 1, this.transform.rotation);
        }
    }
}
