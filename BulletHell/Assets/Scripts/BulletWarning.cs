using UnityEngine;
using System.Collections;

public class BulletDetection : MonoBehaviour
{
    public float detectionRadius = 5f;  // Radius to detect bullets
    public GameObject circlePrefab;  // Prefab for the circle (for regular bullets)
    public GameObject parryableCirclePrefab;  // Prefab for the circle (for parryable bullets)
    private GameObject currentCircle;  // Reference to the circle object

    private SpriteRenderer circleRenderer;  // SpriteRenderer to control the circle's opacity
    private bool isFading = false;  // To check if fade coroutine is already running

    public float fadeDuration = 1f;  // Time to fade in/out the circle

    private void Update()
    {
        // Find all bullets in the scene within the detection radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        bool bulletNearby = false;
        bool parryableBulletNearby = false;

        // Loop through all objects within the radius to check if any are bullets
        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Bullet"))
            {
                bulletNearby = true;
                break;
            }
            // Check if the bullet has the "ParryableBullet" tag
            if (collider.CompareTag("ParryableBullet"))
            {
                parryableBulletNearby = true;
                break;
            }
        }

        // If a parryable bullet is nearby, show the corresponding circle prefab
        if (parryableBulletNearby)
        {
            if (currentCircle == null)
            {
                // Instantiate the circle for the parryable bullet
                currentCircle = Instantiate(parryableCirclePrefab, transform.position, Quaternion.identity);
                currentCircle.transform.SetParent(transform);  // Keep the circle relative to the player

                // Get the SpriteRenderer component of the circle
                circleRenderer = currentCircle.GetComponent<SpriteRenderer>();

                // Start fading in the circle
                if (!isFading)
                    StartCoroutine(FadeInCircle());
            }
        }
        // If a regular bullet is nearby, show the regular circle prefab
        else if (bulletNearby)
        {
            if (currentCircle == null)
            {
                // Instantiate the regular circle
                currentCircle = Instantiate(circlePrefab, transform.position, Quaternion.identity);
                currentCircle.transform.SetParent(transform);  // Keep the circle relative to the player

                // Get the SpriteRenderer component of the circle
                circleRenderer = currentCircle.GetComponent<SpriteRenderer>();

                // Start fading in the circle
                if (!isFading)
                    StartCoroutine(FadeInCircle());
            }
        }
        else
        {
            // If no bullet is nearby, fade out the circle
            if (currentCircle != null)
            {
                if (!isFading)
                    StartCoroutine(FadeOutCircle());
            }
        }

        // Update the position of the circle to follow the player
        if (currentCircle != null)
        {
            currentCircle.transform.position = transform.position;
        }
    }

    // Coroutine to fade the circle in
    private IEnumerator FadeInCircle()
    {
        isFading = true;

        float elapsedTime = 0f;
        Color startColor = circleRenderer.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);  // Fully opaque

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            circleRenderer.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            yield return null;
        }

        circleRenderer.color = targetColor;
        isFading = false;
    }

    // Coroutine to fade the circle out
    private IEnumerator FadeOutCircle()
    {
        isFading = true;

        float elapsedTime = 0f;
        Color startColor = circleRenderer.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);  // Fully transparent

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            circleRenderer.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            yield return null;
        }

        circleRenderer.color = targetColor;
        Destroy(currentCircle);  // Destroy the circle once it is fully transparent
        isFading = false;
    }
}
