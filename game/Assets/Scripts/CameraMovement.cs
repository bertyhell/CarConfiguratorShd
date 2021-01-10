using UnityEngine;

public class CameraMovement : MonoBehaviour {
    [SerializeField] float horRotationSpeed = 150f;
    [SerializeField] float vertRotationSpeed = 100f;

    [SerializeField] float minVerticalAngle = -18f;
    [SerializeField] float maxVerticalAngle = 28f;


    private float rotationX = 0;
    private float rotationY = 0;
    private float rotationZ = 0;

    private bool isEnabled = true;

    void Start() {
        rotationX = transform.rotation.eulerAngles.x;
        rotationY = transform.rotation.eulerAngles.y;
        rotationZ = transform.rotation.eulerAngles.z;
    }

    void Update() {
        if (!isEnabled) {
            return; // Camera control is given to the Viewport script
        }
        if (Input.GetButton("MouseButton1")) {
            //CLAMPS
            float mouseDeltaX = Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1);
            float mouseDeltaY = Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1);
            float mouseX = mouseDeltaX * horRotationSpeed * Time.deltaTime;
            float mouseY = mouseDeltaY * vertRotationSpeed * Time.deltaTime;

            rotationX -= mouseY;
            rotationY += mouseX;

            transform.rotation = Quaternion.Euler(
                Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle), // Avoid going through the ground
                (rotationY + 360) % 360,
                rotationZ
            );
        }
    }

    public void SetEnabled(bool enabled) {
        isEnabled = enabled;
    }
}
