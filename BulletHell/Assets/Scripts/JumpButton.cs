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

    void Start()
    {
        rb = Player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Perform the ground check using OverlapCircle or Raycast
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, GroundLayer);

        // Only allow jumping if on the ground and the button is pressed
        if (Pressed && isGrounded)
        {
            rb.AddForce(new Vector2(0, JumpSpeed), ForceMode2D.Impulse);
            Pressed = false;  // Stop applying the force once it has been triggered
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Optionally, you can leave this empty if you don't need to handle the button release.
    }
}