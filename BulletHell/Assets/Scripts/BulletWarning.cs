using UnityEngine;

public class BulletDetection : MonoBehaviour
{
    public float detectionRadius = 5f;  // Radius to detect bullets
    public GameObject circlePrefab;  // Prefab for the circle (should be a 2D circle sprite)
    private GameObject currentCircle;  // Reference to the circle object

    private void Update()
    {
        // Find all bullets in the scene within the detection radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        bool bulletNearby = false;

        // Loop through all objects within the radius to check if any are bullets
        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Bullet"))
            {
                bulletNearby = true;
                break;
            }
        }

        // If a bullet is nearby, show the circle, else hide it
        if (bulletNearby)
        {
            if (currentCircle == null)
            {
                // Create the circle around the player if it doesn't exist
                currentCircle = Instantiate(circlePrefab, transform.position, Quaternion.identity);
                currentCircle.transform.SetParent(transform);  // Keep the circle relative to the player
            }
        }
        else
        {
            if (currentCircle != null)
            {
                // Hide the circle if no bullet is nearby
                Destroy(currentCircle);
            }
        }

        // Update the position of the circle to follow the player
        if (currentCircle != null)
        {
            currentCircle.transform.position = transform.position;
        }
    }
}
