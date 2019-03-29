using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class MouseCamera {

    // Variables declaration.
    [SerializeField] private string xMouseInputName;
    [SerializeField] private string yMouseInputName;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float smoothTime;

    private Quaternion playerTargetRotation;
    private Quaternion cameraTargetRotation;
    // End of variables declaration.

    // MouseCamera initialization.
    public void Init(Transform player, Transform camera) {
        playerTargetRotation = player.localRotation;
        cameraTargetRotation = camera.localRotation;
        GameObject.Find("Object Text").GetComponent<Text>().text = "";
    }

    // Rotates the camera on the X Axis and the player on the Y Axis.
    public void CameraRotation(Transform player, Transform camera) {
        float yRotation = Input.GetAxis(xMouseInputName) * mouseSensitivity;
        float xRotation = Input.GetAxis(yMouseInputName) * mouseSensitivity;

        playerTargetRotation *= Quaternion.Euler(0f, yRotation, 0f);
        cameraTargetRotation *= Quaternion.Euler(-xRotation, 0f, 0f);

        cameraTargetRotation = ClampXRotation(cameraTargetRotation); // Clamps the camera rotation on the X Axis.

        player.localRotation = Quaternion.Slerp(player.localRotation, playerTargetRotation, smoothTime * Time.fixedDeltaTime);
        camera.localRotation = Quaternion.Slerp(camera.localRotation, cameraTargetRotation, smoothTime * Time.fixedDeltaTime);

        LockCursor();
    }

    // Lock the cursor to the center of the screen.
    private void LockCursor() => Cursor.lockState = CursorLockMode.Locked;

    // Method to clamp the camera rotation on the X Axis.
    Quaternion ClampXRotation(Quaternion q) {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp (angleX, -90.0f, 90.0f);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
    
}