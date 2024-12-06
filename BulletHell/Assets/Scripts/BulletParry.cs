using UnityEngine;

public class DestroyBulletsWithCircle : MonoBehaviour
{
    // The radius of the circle when left-clicking
    public float circleRadius = 3f;

    // Cooldown time in seconds (optional, if you want to limit how often the player can click)
    public float cooldownTime = 1f;

    // Timer to track cooldown
    private float cooldownTimer = 0f;

    // The player's position (this will be the center of the circle)
    private Vector2 playerPosition;

    void Update()
    {
        // Update the cooldown timer
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Update the player's position
        playerPosition = transform.position;

        // Check for left mouse button click and ensure cooldown has expired
        if (Input.GetMouseButtonDown(0) && cooldownTimer <= 0f)
        {
            // Perform the detection and destroy bullets within the circle around the player
            DestroyBulletsInCircle();

            // Reset the cooldown timer
            cooldownTimer = cooldownTime;
        }
    }

    void DestroyBulletsInCircle()
    {
        // Find all colliders within the circle around the player
        Collider2D[] colliders = Physics2D.OverlapCircleAll(playerPosition, circleRadius);

        // Iterate through each collider
        foreach (Collider2D collider in colliders)
        {
            // If the collider is a bullet, destroy it
            if (collider.CompareTag("Bullet"))
            {
                Destroy(collider.gameObject);
            }
        }
    }

    // Optional: To visualize the circle in the scene (for debugging purposes)
    void OnDrawGizmos()
    {
        // Only draw the circle in the editor
        if (cooldownTimer <= 0f)
        {
            // Set the Gizmos color to red
            Gizmos.color = Color.red;

            // Draw the circle at the player's position
            Gizmos.DrawWireSphere(playerPosition, circleRadius);
        }
    }
}