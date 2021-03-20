using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform CharacterPos;
    public float MouseSensitivity = 100f;

    float XRotation = 0f;

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;

        XRotation -= mouseY;
        XRotation = Mathf.Clamp(XRotation, -45f, 45f);
        Debug.Log(XRotation);

        transform.localRotation = Quaternion.Euler(XRotation, 0f, 0f);
        CharacterPos.Rotate(Vector3.up * mouseX);

        float yRotation = Mathf.Clamp(CharacterPos.eulerAngles.y, 120, 240);
        CharacterPos.eulerAngles = new Vector3(0, yRotation, 0);

        Debug.Log(yRotation);
    }
}
