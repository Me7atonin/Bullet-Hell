using UnityEngine;

public class DestroyBulletsWithCircle : MonoBehaviour
{
    // The radius of the circle when left-clicking
    public float circleRadius = 3f;

    // Cooldown time in seconds (optional, if you want to limit how often the player can click)
    public float cooldownTime = 1f;

    // Timer to track cooldown
    private float cooldownTimer = 0f;

    void Update()
    {
        // Update the cooldown timer
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0) && cooldownTimer <= 0f)
        {
            // Perform the detection and destroy bullets
            DestroyBulletsInCircle();

            // Reset the cooldown timer
            cooldownTimer = cooldownTime;
        }
    }

    void DestroyBulletsInCircle()
    {
        // Get the position of the mouse in world space
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Find all colliders within the circle at the mouse position
        Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePosition, circleRadius);

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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, circleRadius);
    }
}
