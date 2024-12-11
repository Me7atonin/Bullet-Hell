using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JumpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool Pressed = false;
    public GameObject Player;
    public float JumpSpeed = 5f;
    public LayerMask GroundLayer; // Add a LayerMask for the ground layer
    private Rigidbody2D rb;
    private bool isGrounded;

    public Transform GroundCheck; // Reference to the transform where the ground check will happen
    public float GroundCheckRadius = 0.2f; // Radius of the ground check

    public Transform WallCheckLeft; // Reference to the left wall check
    public Transform WallCheckRight; // Reference to the right wall check
    public float WallCheckRadius = 0.2f; // Radius for wall checks
    public LayerMask WallLayer; // LayerMask for walls

    private bool canWallJump = false;
    private bool wallJumped = false; // To ensure we only wall jump once per wall

    void Start()
    {
        rb = Player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Perform the ground check using OverlapCircle
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, GroundLayer);

        // Check for wall contact
        bool isTouchingWallLeft = Physics2D.OverlapCircle(WallCheckLeft.position, WallCheckRadius, WallLayer);
        bool isTouchingWallRight = Physics2D.OverlapCircle(WallCheckRight.position, WallCheckRadius, WallLayer);

        // Allow wall jump if touching a wall and not already wall jumping
        if ((isTouchingWallLeft || isTouchingWallRight) && !isGrounded && !wallJumped)
        {
            canWallJump = true;
        }
        else
        {
            canWallJump = false;
        }

        // Handle jumping based on button press
        if (Pressed)
        {
            if (isGrounded)
            {
                // Jump when grounded
                rb.AddForce(new Vector2(0, JumpSpeed), ForceMode2D.Impulse);
                Pressed = false;
            }
            else if (canWallJump)
            {
                // Wall jump logic: apply force in the opposite direction of the wall
                Vector2 wallJumpDirection = Vector2.zero;
                if (isTouchingWallLeft)
                {
                    wallJumpDirection = new Vector2(1, 1); // Jump right
                }
                else if (isTouchingWallRight)
                {
                    wallJumpDirection = new Vector2(-1, 1); // Jump left
                }

                rb.velocity = new Vector2(0, 0); // Reset velocity to avoid unnatural jumps
                rb.AddForce(wallJumpDirection.normalized * JumpSpeed, ForceMode2D.Impulse);
                wallJumped = true;  // Prevent further wall jumping on the same wall
                Pressed = false;
            }
        }

        // Reset wall jump state when no longer touching the wall
        if (!isTouchingWallLeft && !isTouchingWallRight)
        {
            wallJumped = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // You can leave this empty or handle button release if needed
    }
}
