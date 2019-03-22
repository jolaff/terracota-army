using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    // Movement variables
    [SerializeField] string horizontalInputName;
    [SerializeField] string verticalInputName;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float runBuildUpSpeed;
    private float velocity;
    
    // Jump variables
    [SerializeField] float jumpMultiplier;
    [SerializeField] AnimationCurve jumpFallOff;
    private bool isJumping;

    // Slope fixing variables
    [SerializeField] float slopeForce;
    [SerializeField] float slopeForceRayLength;


    // KeyCode declarations
    [SerializeField] KeyCode runKey;
    [SerializeField] KeyCode jumpKey;

    // Cached references
    private CharacterController characterController;

    // Initialization
    private void Awake() {
        characterController = GetComponent<CharacterController>();
    }

    // Runs every frame
    private void Update() {
        PlayerMovement();
    }

    // Controls the movement (WASD/Arrow keys), running (LEFT SHIFT) and jumping (SPACE)
    private void PlayerMovement() {
        float horInput = Input.GetAxis(horizontalInputName);
        float vertInput = Input.GetAxis(verticalInputName);

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horInput;

        characterController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * velocity); // Fix diagonal movement speed

        if ((vertInput != 0 || horInput != 0) && OnSlope()) characterController.Move(Vector3.down * characterController.height / 2 * slopeForce * Time.fixedDeltaTime); // Call OnSlope() method

        SetVelocity();
        JumpInput();
    }

    // Running / Walking
    private void SetVelocity() {
        if (Input.GetKey(runKey)) velocity = Mathf.Lerp(velocity, runSpeed, Time.fixedDeltaTime * runBuildUpSpeed);
        else velocity = Mathf.Lerp(velocity, walkSpeed, Time.fixedDeltaTime * runBuildUpSpeed);
    }

    // Fixing slope bouncing and jittering
    private bool OnSlope() {
        if (isJumping) return false;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, characterController.height / 2 * slopeForceRayLength)) {
            if (hit.normal != Vector3.up) return true;
        }
        return false;
    }

    // Gets the Jump key (SPACE)
    private void JumpInput() {
        if (Input.GetKeyDown(jumpKey) && !isJumping) {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }

    // Jump method **  TODO: check small delay when jumping while not moving
    private IEnumerator JumpEvent() {

        characterController.slopeLimit = 90.0f;
        float timeInAir = 0.0f;

        do {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            characterController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.fixedDeltaTime);
            timeInAir += Time.fixedDeltaTime;
            yield return null;
        } while (!characterController.isGrounded && characterController.collisionFlags != CollisionFlags.Above);

        characterController.slopeLimit = 45.0f;
        isJumping = false;
    }

}