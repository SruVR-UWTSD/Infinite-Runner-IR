using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 10f;          // Constant forward speed
    public float strafeSpeed = 6f;            // Speed for moving left & right

    [Header("Jumping")]
    public float jumpForce = 8f;              // How high the player jumps
    public float gravity = -20f;              // Custom gravity for tight control

    [Header("Sliding")]
    public float slideDuration = 0.6f;        // How long the slide lasts
    private bool isSliding = false;
    private float slideTimer = 0f;

    [Header("Character Controller")]
    public CharacterController controller;    // For collision + smooth movement

    // Internal states
    private Vector3 velocity;                 // Movement vector for gravity/jump
    private bool isGrounded = true;
    private Vector3 startPosition;            // For ResetPlayer()

    // INPUT
    private Gamepad gamepad;

    void Start()
    {
        startPosition = transform.position;

        if (controller == null)
            controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        gamepad = Gamepad.current;
        if (gamepad == null) return;

        // -----------------------
        // CONSTANT FORWARD MOTION
        // -----------------------
        Vector3 move = transform.forward * forwardSpeed;

        // -----------------------
        // LEFT / RIGHT STRAFING
        // -----------------------
        float leftRight = gamepad.leftStick.x.ReadValue();
        move += transform.right * (leftRight * strafeSpeed);

        // -----------------------
        // JUMP
        // A button = buttonSouth
        // -----------------------
        if (isGrounded && gamepad.buttonSouth.wasPressedThisFrame)
        {
            velocity.y = jumpForce;
            isGrounded = false;
        }

        // -----------------------
        // SLIDE / DUCK
        // B button = buttonEast
        // -----------------------
        if (isGrounded && gamepad.buttonEast.wasPressedThisFrame && !isSliding)
        {
            StartSlide();
        }

        // Sliding countdown
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0f)
            {
                StopSlide();
            }
        }

        // -----------------------
        // GRAVITY
        // -----------------------
        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // -----------------------
        // APPLY MOVEMENT
        // -----------------------
        controller.Move((move + velocity) * Time.deltaTime);

        // Check grounding
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            isGrounded = true;
        }
    }

    // ============================================================
    // SLIDE FUNCTIONS
    // ============================================================

    void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;

        // SHRINK controller height to duck under obstacles
        controller.height = 0.8f;
        controller.center = new Vector3(0, 0.4f, 0);
    }

    void StopSlide()
    {
        isSliding = false;

        // RESET controller height
        controller.height = 2f;
        controller.center = new Vector3(0, 1f, 0);
    }

    // ============================================================
    // RESET PLAYER FOR RESTART / AFTER DEATH
    // ============================================================

    public void ResetPlayer()
    {
        // Reset player to original position (start of track)
        transform.position = startPosition;

        // Reset velocity
        velocity = Vector3.zero;

        // Reset sliding state
        StopSlide();

        // Reset grounding
        isGrounded = true;
    }
}
