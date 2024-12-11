using UnityEngine;

public class WallJump : MonoBehaviour
{
    public float moveSpeed = 5f; // The speed of the player movement
    public float jumpForce = 10f; // The force of the player's jump
    public float wallJumpForce = 12f; // The force of the wall jump
    public float wallSlideSpeed = 2f; // The speed of the player sliding on the wall
    public int maxWallJumps = 2; // Maximum number of wall jumps allowed
    private int currentWallJumps = 0; // The current number of wall jumps the player has used

    private bool isTouchingWall = false; // Whether the player is touching a wall
    private bool isGrounded = false; // Whether the player is grounded
    private bool isWallSliding = false; // Whether the player is sliding down a wall
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    // Wall detection parameters
    public float wallCheckDistance = 0.5f; // Distance to check if the player is touching the wall
    public LayerMask wallLayer; // Layer to detect walls (set in the Inspector)

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        isGrounded = CheckGrounded();
        isTouchingWall = CheckWall();

        if (isTouchingWall && !isGrounded)
        {
            HandleWallSlide();
        }

        // Perform wall jump if the player is touching a wall and hasn't used all jumps
        if (isTouchingWall && !isGrounded && Input.GetButtonDown("Jump") && currentWallJumps < maxWallJumps)
        {
            WallJumpAction();
        }

        // Reset wall jumps when the player touches the ground
        if (isGrounded)
        {
            currentWallJumps = 0;
        }
    }

    private bool CheckGrounded()
    {
        // Check if the player is touching the ground using a simple box collider check
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
    }

    private bool CheckWall()
    {
        // Check if the player is touching a wall using raycasts to the left and right
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, wallLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, wallLayer);

        return hitLeft.collider != null || hitRight.collider != null;
    }

    private void HandleWallSlide()
    {
        // If the player is sliding down a wall and isn't pressing jump, apply wall sliding
        if (rb.velocity.y < 0 && !Input.GetButton("Jump"))
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJumpAction()
    {
        // Determine the direction to jump. Jump backwards from the wall.
        float wallDirection = 0f;

        if (isTouchingWall)
        {
            // Check which side of the wall the player is on and apply jump force in the opposite direction
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, wallLayer);
            RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, wallLayer);

            if (hitLeft.collider != null)
            {
                // Jump to the right (away from the wall)
                wallDirection = 1f;
            }
            else if (hitRight.collider != null)
            {
                // Jump to the left (away from the wall)
                wallDirection = -1f;
            }
        }

        // Apply wall jump force: add horizontal force (away from the wall) and vertical force (jumping)
        rb.velocity = new Vector2(wallDirection * wallJumpForce, jumpForce);

        // Increment the wall jump count
        currentWallJumps++;

        // Optionally, you can play a wall jump sound or animation here
    }

    private void OnDrawGizmos()
    {
        // Visualize the wall detection area (optional, for debugging)
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.left * wallCheckDistance);
        Gizmos.DrawRay(transform.position, Vector2.right * wallCheckDistance);
    }
}