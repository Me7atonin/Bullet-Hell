using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    public float speed = 10f;  // Speed of the bullet
    public float homingStrength = 5f;  // How strong the homing behavior is
    private Transform target;  // Reference to the player's transform (target for homing)

    private void Start()
    {
        // Search for the player object by tag
        target = GameObject.FindGameObjectWithTag("Player")?.transform;

        // If no player is found, log a warning
        if (target == null)
        {
            Debug.LogWarning("No player object found. The homing bullet will not function properly.");
        }
    }

    private void Update()
    {
        if (target != null)
        {
            // Calculate the direction to the player
            Vector3 directionToPlayer = (target.position - transform.position).normalized;

            // Smoothly rotate towards the player using Slerp
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, homingStrength * Time.deltaTime);

            // Move the bullet forward in the direction of its current rotation
            transform.position += transform.up * speed * Time.deltaTime;  // Move the bullet in the new direction
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Handle collision with player or other objects
        if (other.CompareTag("Player"))
        {
            // Destroy the bullet when it hits the player
            Destroy(gameObject);
        }

        // Add additional collision checks if needed (e.g., for walls or obstacles)
    }
}
