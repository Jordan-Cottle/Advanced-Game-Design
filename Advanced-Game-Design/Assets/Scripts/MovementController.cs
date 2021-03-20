using UnityEngine;

public class MovementController : MonoBehaviour
{
    // Start is called before the first frame update
    public float MovementSpeed = 15f;
    public float Gravity = -10f;
    public CharacterController Controller;
    public Transform GroundCheck;
    public float GroundDistance;
    public LayerMask GroundMask;

    bool grounded;
    Vector3 velocity;

    // Update is called once per frame
    void Update()
    {

        if (Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask))
        {
            velocity.y = 0;
        }

        float horizontalInput = Input.GetAxis("Horizontal") * MovementSpeed * Time.deltaTime;
        float verticalInput = Input.GetAxis("Vertical") * MovementSpeed * Time.deltaTime;


        Controller.Move(new Vector3(-horizontalInput, verticalInput, 0));
    }
}
