using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform CharacterPos;
    public float MouseSensitivity = 100f;

    float xRotation = 0f;
    float yRotation = 0f;

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;

        xRotation = Mathf.Clamp(xRotation + mouseX, -45f, 45f);
        yRotation = Mathf.Clamp(yRotation - mouseY, -60f, 60f);

        transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        CharacterPos.rotation = Quaternion.Euler(0f, xRotation, 0f);
    }
}
