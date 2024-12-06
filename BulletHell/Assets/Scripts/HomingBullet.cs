using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    // Reference to the player
    public Transform player;

    // Speed of the bullet
    public float speed = 5f;

    // Homing strength (how fast it adjusts its direction)
    public float homingStrength = 5f;

    // Optionally, set a maximum homing angle to avoid the bullet spinning too aggressively
    public float maxHomingAngle = 45f;

    void Start()
    {
        // If no player is assigned, try to find the player by tag
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
    }

    void Update()
    {
        // Ensure the player exists before calculating direction
        if (player != null)
        {
            // Calculate the direction vector from the bullet to the player
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            // Calculate the angle between the bullet's current direction and the target direction
            float angle = Vector3.Angle(transform.up, directionToPlayer);

            // Apply the homing strength to adjust the bullet's velocity
            if (angle < maxHomingAngle)
            {
                // Rotate the bullet's direction smoothly toward the player
                transform.up = Vector3.RotateTowards(transform.up, directionToPlayer, homingStrength * Time.deltaTime, 0f);
            }

            // Move the bullet in the direction it is facing
            transform.position += transform.up * speed * Time.deltaTime;
        }
    }
}