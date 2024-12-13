using UnityEngine;
using UnityEngine.UI;  // Required for UI Text
using UnityEngine.SceneManagement;  // Required for scene management
using System.Collections;  // Required for using Coroutines

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;  // Player's initial health
    public float damageMultiplier = 1f;  // Multiplier to scale damage taken
    public Text healthText;  // Reference to the UI Text component that displays health
    public float immunityTime = 1f;  // Duration of immunity time in seconds

    private bool isImmune = false;  // Flag to check if player is immune to damage

    private void Start()
    {
        // Update health display at the start of the game
        UpdateHealthUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that collided is a bullet
        if (other.CompareTag("Bullet") && !isImmune)
        {
            // Get the bullet's damage from the Bullet script
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

        // Update the health UI whenever the player takes damage
        UpdateHealthUI();

        // Start immunity timer
        StartCoroutine(ImmuneTimer());

        // Check if the player's health reaches zero or below
        if (health <= 0)
        {
            Die();
        }
    }

    // Coroutine for immunity timer
    IEnumerator ImmuneTimer()
    {
        isImmune = true;  // Set immunity flag to true
        yield return new WaitForSeconds(immunityTime);  // Wait for the immunity duration
        isImmune = false;  // Set immunity flag to false after the timer expires
    }

    // Method to handle player's death and load a different scene
    void Die()
    {
        Debug.Log("Player is dead!");

        // Load the "GameOver" scene or any other scene of your choice
        // Replace "GameOver" with the name of your desired scene
        SceneManager.LoadScene("GameOver");
    }

    // Method to update the health UI text
    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + Mathf.Max(0, health).ToString("F0");  // Display health as a whole number
        }
    }
}