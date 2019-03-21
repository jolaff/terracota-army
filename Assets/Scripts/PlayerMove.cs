using UnityEngine;

public class PlayerMove : MonoBehaviour {
    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;
    [SerializeField] private float velocity;

    private CharacterController characterController;

    private void Awake() {
        characterController = GetComponent<CharacterController>();
    }

    private void Update() {
        PlayerMovement();
    }

    private void PlayerMovement() {
        float horInput = Input.GetAxis(horizontalInputName) * velocity;
        float vertInput = Input.GetAxis(verticalInputName) * velocity;

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horInput;

        characterController.SimpleMove(forwardMovement + rightMovement);
    }

}