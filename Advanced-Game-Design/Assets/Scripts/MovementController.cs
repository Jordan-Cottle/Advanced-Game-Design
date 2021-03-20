using UnityEngine;

public class MovementController : MonoBehaviour
{
    // Start is called before the first frame update
    public float MovementSpeed = 15f;
    public float Acceleration = 1f;
    public CharacterController Controller;
    bool grounded;
    Vector3 velocity;

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal") * MovementSpeed;
        float verticalInput = Input.GetAxis("Vertical") * MovementSpeed;

        velocity.x = -horizontalInput;
        velocity.y = verticalInput;
        velocity.z -= Acceleration * Time.deltaTime;

        Controller.Move(velocity * Time.deltaTime);
    }
}
