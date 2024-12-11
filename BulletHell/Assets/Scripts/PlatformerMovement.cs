using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerMovement : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5f;
    [SerializeField]
    float jumpSpeed = 2f;
    bool grounded = false;
    Rigidbody2D rb;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get horizontal movement input from either keyboard or mobile joystick
        float moveX = Input.GetAxis("Horizontal"); // Handles both keyboard and mobile joystick input

        Vector2 velocity = rb.velocity;
        velocity.x = moveX * moveSpeed; // Apply movement to the Rigidbody2D
        rb.velocity = velocity;

        // Handle jumping (both keyboard and mobile)
        if (Input.GetButtonDown("Jump") && grounded) // "Jump" works for both keyboard space and mobile buttons
        {
            rb.AddForce(new Vector2(0, 100 * jumpSpeed));
            grounded = false;
        }

        // Set animation parameters
        //anim.SetFloat("y", velocity.y);
        //anim.SetBool("grounded", grounded);

        int x = (int)Input.GetAxisRaw("Horizontal"); // Get raw horizontal input (either -1, 0, or 1)
        //anim.SetInteger("x", x);

        // Flip sprite based on direction (for left-right movement)
        //if (x > 0)
        {
           // GetComponent<SpriteRenderer>().flipX = false;
        }
        //else if (x < 0)
        {
            //GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    // Detect when the player touches the ground (e.g., collides with the ground layer)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6) // Layer 6 represents the ground
        {
            grounded = true;
        }
    }

    // Detect when the player leaves the ground
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6) // Layer 6 represents the ground
        {
            grounded = false;
        }
    }
}