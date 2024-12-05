using UnityEngine;

public class SpinningShooter : MonoBehaviour
{
    public GameObject bulletPrefab;  // Reference to the bullet prefab
    public float spinSpeed = 100f;   // Speed of spinning in degrees per second
    public float shootSpeed = 10f;   // Speed of bullet movement
    public float shootInterval = 0.1f; // Interval between each shot
    public int numberOfBullets = 8; // Number of bullets to shoot in a full rotation

    private float shootTimer = 0f;   // Timer to track shooting interval

    void Update()
    {
        // Spin the object continuously around the Z-axis
        transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime);

        // Handle shooting
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootInterval)
        {
            ShootBulletsInCircle();
            shootTimer = 0f; // Reset the timer after shooting
        }
    }

    void ShootBulletsInCircle()
    {
        // Calculate the angle between each bullet
        float angleStep = 360f / numberOfBullets;

        // Spawn bullets in a circular pattern
        for (int i = 0; i < numberOfBullets; i++)
        {
            // Calculate the angle for this bullet
            float angle = i * angleStep + transform.eulerAngles.z;

            // Convert angle to radians for calculation
            float radians = Mathf.Deg2Rad * angle;

            // Calculate the direction of the bullet based on the angle
            Vector2 shootDirection = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

            // Instantiate the bullet at the object's position
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            // Set the velocity of the bullet
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = shootDirection * shootSpeed;
            }
        }
    }
}