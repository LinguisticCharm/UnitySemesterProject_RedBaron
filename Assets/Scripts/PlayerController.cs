using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // all my lovely defines...
    private float acceleration;
    public float walkSpeed;
    public float runSpeed;
    public float jumpForce;
    public LayerMask groundLayer;
    Rigidbody2D rb;
    Collider2D playerCollider;
    private InputAction _move;
    private InputAction _run;
    private InputAction _jump;
    AudioSource jumpSound;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        jumpSound = GetComponentInChildren<AudioSource>();
        acceleration = 0;

        _move = InputSystem.actions.FindAction("Move");
        _run = InputSystem.actions.FindAction("Run");
        _jump = InputSystem.actions.FindAction("Jump");
    }
    private bool startJump;
    private bool heldJump;

    private void Update()
    {
        heldJump = _jump.IsPressed();
        if (_jump.WasPressedThisFrame())
            startJump = true;
    }
    
    private void FixedUpdate()
    {
        Vector2 moveInput = _move.ReadValue<Vector2>();

        float speed = (_run.IsPressed()) ? runSpeed : walkSpeed;

        float targetSpeed = moveInput.x * speed;
        acceleration = Mathf.MoveTowards(acceleration, targetSpeed, 0.75f);

        rb.linearVelocity = new Vector2(acceleration, rb.linearVelocity.y);

        if (startJump && playerCollider.IsTouchingLayers(groundLayer))
        {
            startJump = false;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpSound.Play(); //dwoooing
        }
        if (!playerCollider.IsTouchingLayers(groundLayer))
        {
            rb.gravityScale = heldJump ? 3.5f : 6.5f;
        }
    }
}