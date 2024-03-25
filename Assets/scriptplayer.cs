using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 5f; // Maximum speed of player movement
    public float acceleration = 10f; // Acceleration of player movement
    public float deceleration = 10f; // Deceleration of player movement
    public float jumpForce = 10f; // Force of player jump
    public float rotationPerDistance = 360f; // Rotation degrees per unit of movement
    public LayerMask groundLayer; // Layer mask for detecting ground
    public CircleCollider2D cc;

    private Rigidbody2D rb;
    private Vector2 lastPosition;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastPosition = rb.position;
    }

    void Update()
    {
        MoveInput();
        Jump();
    }

    void FixedUpdate()
    {
        RotatePlayer();
    }

    void MoveInput()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        float newVelocityX = rb.velocity.x + moveInput * acceleration * Time.fixedDeltaTime;

        if (moveInput == 0)
        {
            // Decelerate smoothly towards a complete stop
            newVelocityX -= Mathf.Sign(rb.velocity.x) * deceleration * Time.fixedDeltaTime;
        }

        // Clamp velocity to stay within maxSpeed
        rb.velocity = new Vector2(Mathf.Clamp(newVelocityX, -maxSpeed, maxSpeed), rb.velocity.y);

        // Update last position
        lastPosition = rb.position;
    }

    void RotatePlayer()
    {
        // Calculate movement distance
        float movementDistance = Vector2.Distance(rb.position, lastPosition);

        // Calculate rotation angle based on movement distance
        float rotationAngle = (movementDistance / (2 * Mathf.PI * transform.localScale.x)) * 360f;

        // Apply rotation to the player
        transform.Rotate(Vector3.forward, -rotationAngle);
    }

    void Jump()
    {
        // Check if the player is grounded
        isGrounded = cc.IsTouchingLayers(groundLayer);
        // Check if the player is grounded
        // isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);


        // Jump input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}
