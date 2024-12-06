using UnityEngine;

public class DestroyBullets : MonoBehaviour
{
    // Optionally, specify a tag for the bullets, such as "Bullet" in the Inspector
    public string bulletTag = "Bullet";

    // When another object enters the trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that collided is tagged as "Bullet"
        if (other.CompareTag(bulletTag))
        {
            // Destroy the bullet object
            Destroy(other.gameObject);
        }
    }

    // Optionally, you can also use OnCollisionEnter2D for non-trigger colliders
    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag(bulletTag))
    //     {
    //         Destroy(collision.gameObject);
    //     }
    // }
}
