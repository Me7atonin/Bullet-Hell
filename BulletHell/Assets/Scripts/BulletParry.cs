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

    // AudioSource to play sound when a bullet is destroyed
    public AudioSource destructionSound;

    // Prefab to instantiate when a bullet is destroyed (this should have an animation)
    public GameObject destructionEffectPrefab;

    // Offset position for the destruction effect (you can adjust this if you want the effect to appear somewhere specific)
    public Vector2 effectOffset = new Vector2(0, 0);

    // Duration in seconds for how long the effect should remain before disappearing
    public float effectLifetime = 1f;

    // Reference to the instantiated destruction effect
    private GameObject effectInstance;

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

        // Make the effect follow the player (if it exists)
        if (effectInstance != null)
        {
            effectInstance.transform.position = playerPosition + (Vector2)effectOffset;
        }
    }

    void DestroyBulletsInCircle()
    {
        // Find all colliders within the circle around the player
        Collider2D[] colliders = Physics2D.OverlapCircleAll(playerPosition, circleRadius);

        // A flag to check if any bullet was destroyed
        bool bulletDestroyed = false;

        // Iterate through each collider
        foreach (Collider2D collider in colliders)
        {
            // If the collider is a bullet, destroy it
            if (collider.CompareTag("ParryableBullet"))
            {
                // Destroy the bullet
                Destroy(collider.gameObject);

                // Mark that a bullet was destroyed
                bulletDestroyed = true;
            }
        }

        // If a bullet was destroyed, play the destruction sound and instantiate the destruction effect
        if (bulletDestroyed)
        {
            // Play the destruction sound if assigned
            if (destructionSound != null)
            {
                destructionSound.Play();
            }

            // Instantiate the destruction effect prefab if assigned
            if (destructionEffectPrefab != null)
            {
                // Instantiate the effect at the player's position (with the specified offset)
                effectInstance = Instantiate(destructionEffectPrefab, playerPosition + (Vector2)effectOffset, Quaternion.identity);

                // Destroy the effect after the specified lifetime
                Destroy(effectInstance, effectLifetime);
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
