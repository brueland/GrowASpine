using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float rotationSpeed = 100.0f;
    public float panSpeed = 20f;
    public float zoomSpeed = 20f;

    void Update()
    {
        // Handle mouse rotation
        if (Input.GetMouseButton(1)) // Right mouse button to rotate
        {
            float newRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float newRotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
        }

        // Handle WASD movement
        float translation = moveSpeed * Time.deltaTime;
        transform.Translate(new Vector3(Input.GetAxis("Horizontal") * translation, 0, Input.GetAxis("Vertical") * translation));

        // Handle mouse panning
        if (Input.GetMouseButton(2)) // Middle mouse button to pan
        {
            float newPosX = transform.position.x - Input.GetAxis("Mouse X") * panSpeed * Time.deltaTime;
            float newPosZ = transform.position.z - Input.GetAxis("Mouse Y") * panSpeed * Time.deltaTime;
            transform.position = new Vector3(newPosX, transform.position.y, newPosZ);
        }

        // Handle mouse zooming
        float zoom = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
        transform.Translate(new Vector3(0, -zoom, 0));
    }
}
