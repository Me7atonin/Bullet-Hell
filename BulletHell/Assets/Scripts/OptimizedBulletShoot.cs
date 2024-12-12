using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    public float shootCooldown = 0.5f;  // Time between each shot
    public float bulletSpeed = 5f;      // Bullet speed
    public float bulletLifetime = 3f;   // How long the bullet lasts before deactivating

    private List<GameObject> bullets;   // List to hold all bullets in the scene
    private float lastShotTime = 0f;    // To track the time of the last shot

    void Start()
    {
        // Find all the bullets in the scene (Make sure these are inactive in the scene)
        bullets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Bullet"));
    }

    void Update()
    {
        // Handle shooting input (for example, space key)
        if (Time.time - lastShotTime >= shootCooldown)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                ShootBullets();
                lastShotTime = Time.time;
            }
        }
    }

    void ShootBullets()
    {
        // Find an inactive bullet to reuse
        GameObject bullet = GetInactiveBullet();
        if (bullet != null)
        {
            bullet.SetActive(true);  // Activate the bullet

            // Position the bullet at the shooter object
            bullet.transform.position = transform.position;

            // Calculate the shooting direction within a 180-degree spread
            float angle = Random.Range(-90f, 90f); // Random angle between -90 and 90 degrees
            Vector3 direction = GetDirectionFromAngle(angle);

            // Apply the movement (shoot in direction)
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed;
            }

            // Start bullet lifetime (deactivates it after a set time)
            StartCoroutine(DeactivateBulletAfterTime(bullet));
        }
    }

    // Get an inactive bullet from the list
    GameObject GetInactiveBullet()
    {
        foreach (GameObject bullet in bullets)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }

        return null;  // If no inactive bullets are found
    }

    // Convert an angle to a direction vector
    Vector3 GetDirectionFromAngle(float angle)
    {
        float radianAngle = angle * Mathf.Deg2Rad;  // Convert to radians
        return new Vector3(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle), 0).normalized;
    }

    // Coroutine to deactivate bullet after lifetime
    IEnumerator DeactivateBulletAfterTime(GameObject bullet)
    {
        yield return new WaitForSeconds(bulletLifetime);
        bullet.SetActive(false);  // Deactivate the bullet
    }
}