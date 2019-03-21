using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private string xMouseInputName;
    [SerializeField] private string yMouseInputName;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform playerBody;

    private float xAxisClamp;
    private void Awake() {
        LockCursor();
        xAxisClamp = 0.0f;
    }

    private void Update() {
        CameraRotation();
    }

    // Lock the cursos to the center of the screen
    private void LockCursor() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Rotates the camera on the Y axis and the player on the X axis
    private void CameraRotation() {
        float xMouse = Input.GetAxis(xMouseInputName) * mouseSensitivity * Time.fixedDeltaTime;
        float yMouse = Input.GetAxis(yMouseInputName) * mouseSensitivity * Time.fixedDeltaTime;

        // Clamps the mouse preventing upside down
        xAxisClamp += yMouse;
        if (xAxisClamp > 90.0f) {
            xAxisClamp = 90.0f;
            yMouse = 0.0f;
            ClampXAxisRotation(270.0f);
        } else if (xAxisClamp < -90.0f) {
            xAxisClamp = -90.0f;
            yMouse = 0.0f;
            ClampXAxisRotation(90.0f);
        }

        // Apply rotation when mouse moves
        transform.Rotate(Vector3.left * yMouse);
        playerBody.Rotate(Vector3.up * xMouse);
    }

    // Limits the rotation to the clamped value
    private void ClampXAxisRotation(float value) {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }

}