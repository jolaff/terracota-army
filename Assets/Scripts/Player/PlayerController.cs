using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour {

    // Variables declaration.
    [SerializeField] private MouseCamera mouseCamera;
    [SerializeField] private CameraBob cameraBob = new CameraBob();
    [SerializeField] private PlayerAudio playerAudio = new PlayerAudio();
    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;
    [SerializeField] private string jumpInputName;
    [SerializeField] private bool isWalking;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField][Range(0f, 1f)] private float runStepLenghten;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float stickToGroundForce;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float stepInterval;
	[HideInInspector] public bool IsWalking { get { return isWalking; } } //Im just gonna borrow that
	public GameObject deathScreen;

    private bool jump;
    private bool jumping;
    private bool previouslyGrounded;
    private float stepCycle;
    private float nextStep;
    private Camera playerCamera;
    private CharacterController characterController;
    private Vector2 input;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 originalCameraPosition;
    private AudioSource audioSource;
    // End of variables declaration.

    // Start is called before the first frame update
    void Start() {
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        playerCamera = Camera.main;
        originalCameraPosition = playerCamera.transform.localPosition;
        cameraBob.Init(playerCamera, stepInterval);
        mouseCamera.Init(transform, playerCamera.transform);
		deathScreen.SetActive(false);
		stepCycle = 0.0f;
        nextStep = stepCycle / 2.0f;
        jumping = false;
    }

    // Update is called once per frame
    void Update() {
        RotateView();

        // Jump states check
        if (!jump && !jumping) jump = Input.GetButtonDown(jumpInputName);
        if (!previouslyGrounded && characterController.isGrounded) {
            StartCoroutine(cameraBob.DoJumpBob());
            playerAudio.PlayLandAudio(audioSource, nextStep, stepCycle);
            moveDirection.y = 0.0f;
            jumping = false;
        }
        if (!characterController.isGrounded && !jumping && previouslyGrounded) {
            moveDirection.y = 0.0f;
        }
        previouslyGrounded = characterController.isGrounded;
    }

    private void FixedUpdate() {
        float speed;
        PlayerMovement(out speed);

        Vector3 requestMove = transform.forward * input.y + transform.right * input.x;

        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out hitInfo, characterController.height / 2.0f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        requestMove = Vector3.ProjectOnPlane(requestMove, hitInfo.normal).normalized;

        moveDirection.x = requestMove.x * speed;
        moveDirection.z = requestMove.z * speed;

        if (characterController.isGrounded) {
            moveDirection.y = -stickToGroundForce;

            if (jump) {
                moveDirection.y = jumpSpeed;
                playerAudio.PlayJumpAudio(audioSource);
                jump = false;
                jumping = true;
            }
        } else {
            moveDirection += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
        }

        characterController.Move(moveDirection * Time.fixedDeltaTime);

        ProgressStepCycle(speed);
        CameraHeadBob(speed);
    }

    private void PlayerMovement(out float speed) {
        // Read input (WASD)
        float horizontalInput = Input.GetAxis(horizontalInputName);
        float verticalInput = Input.GetAxis(verticalInputName);

        // Keep track of whether or not the player is walking or running.
        isWalking = !Input.GetKey(KeyCode.LeftShift);

        // Set the desired speed to be walking or running.
        speed = isWalking?walkSpeed : runSpeed;
        input = new Vector2(horizontalInput, verticalInput);

        // Normalize
        if (input.sqrMagnitude > 1) input.Normalize();
    }

    private void RotateView() => mouseCamera.CameraRotation(transform, playerCamera.transform);

    private void CameraHeadBob(float speed) {
        Vector3 newCameraPosition;
        if (characterController.velocity.magnitude > 0 && characterController.isGrounded) {
            playerCamera.transform.localPosition = cameraBob.DoHeadBob(characterController.velocity.magnitude + (speed * (isWalking ? 1f : runStepLenghten)));
            newCameraPosition = playerCamera.transform.localPosition;
            newCameraPosition.y = playerCamera.transform.localPosition.y - cameraBob.OffSet();
        } else {
            newCameraPosition = playerCamera.transform.localPosition;
            newCameraPosition.y = originalCameraPosition.y - cameraBob.OffSet();
        }
        playerCamera.transform.localPosition = newCameraPosition;
    }

    private void ProgressStepCycle(float speed) {
        if (characterController.velocity.sqrMagnitude > 0 && (input.x != 0 || input.y != 0)) stepCycle += (characterController.velocity.magnitude + (speed * (isWalking?1f : runStepLenghten))) * Time.fixedDeltaTime;
        if (!(stepCycle > nextStep)) return;

        nextStep = stepCycle + stepInterval;

        playerAudio.PlayFootStepAudio(characterController, audioSource, isWalking);
    }

	public void Die()
	{
		deathScreen.SetActive(true);
		Time.timeScale = 0;
	}
}