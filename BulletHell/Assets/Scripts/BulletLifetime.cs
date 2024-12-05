using UnityEngine;

public class BulletLifeTime : MonoBehaviour
{
    public float lifetime = 5f;  // Lifetime of the bullet in seconds

    void Start()
    {
        // Destroy the bullet after the specified lifetime
        Destroy(gameObject, lifetime);
    }
}
