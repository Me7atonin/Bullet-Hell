using UnityEngine;
using UnityEngine.SceneManagement;  // Required for scene management

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;  // Player's initial health
    public float damageMultiplier = 1f;  // Multiplier to scale damage taken

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that collided is a bullet
        if (other.CompareTag("Bullet"))
        {
            // Get the bullet's damage from the Bullet script (which we'll create below)
            float bulletDamage = other.GetComponent<Bullet>().damage;

            // Apply damage to the player's health, using the damage multiplier
            TakeDamage(bulletDamage * damageMultiplier);

            // Destroy the bullet after it hits the player
            Destroy(other.gameObject);
        }
    }

    // Method to take damage
    void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Player Health: " + health);

        // Check if the player's health reaches zero or below
        if (health <= 0)
        {
            Die();
        }
    }

    // Method to handle player's death and load a different scene
    void Die()
    {
        Debug.Log("Player is dead!");

        // Load the "GameOver" scene or any other scene of your choice
        // Replace "GameOver" with the name of your desired scene
        SceneManager.LoadScene("GameOver");
    }
}