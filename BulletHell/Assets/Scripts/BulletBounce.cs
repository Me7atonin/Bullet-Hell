using UnityEngine;

public class BulletBounce : MonoBehaviour
{
    public float bounceFactor = 0.8f;  // A factor to control how much the bullet bounces (1 = perfect bounce, <1 = reduced bounce)

    private Rigidbody2D rb;

    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    // This method is called when the bullet collides with another object
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the normal of the surface where the bullet collided
        Vector2 collisionNormal = collision.contacts[0].normal;

        // Reflect the bullet's velocity based on the collision normal
        Vector2 reflectedVelocity = Vector2.Reflect(rb.velocity, collisionNormal);

        // Apply the reflected velocity with the bounce factor (dampen the bounce if necessary)
        rb.velocity = reflectedVelocity * bounceFactor;
    }
}