using UnityEngine;
using System.Collections;

public class BulletDetection : MonoBehaviour
{
    public float detectionRadius = 5f;  // Radius to detect bullets
    public GameObject circlePrefab;  // Prefab for the circle (for regular bullets)
    public GameObject parryableCirclePrefab;  // Prefab for the circle (for parryable bullets)

    private GameObject regularCircle;  // Reference to the regular bullet circle object
    private GameObject parryableCircle;  // Reference to the parryable bullet circle object

    private SpriteRenderer regularCircleRenderer;  // SpriteRenderer to control the regular circle's opacity
    private SpriteRenderer parryableCircleRenderer;  // SpriteRenderer to control the parryable circle's opacity
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
            }
            // Check if the bullet has the "ParryableBullet" tag
            if (collider.CompareTag("ParryableBullet"))
            {
                parryableBulletNearby = true;
            }
        }

        // If a parryable bullet is nearby, show the corresponding circle prefab
        if (parryableBulletNearby)
        {
            if (parryableCircle == null)
            {
                // Instantiate the circle for the parryable bullet
                parryableCircle = Instantiate(parryableCirclePrefab, transform.position, Quaternion.identity);
                parryableCircle.transform.SetParent(transform);  // Keep the circle relative to the player

                // Get the SpriteRenderer component of the parryable circle
                parryableCircleRenderer = parryableCircle.GetComponent<SpriteRenderer>();

                // Start fading in the circle
                if (!isFading)
                    StartCoroutine(FadeInCircle(parryableCircleRenderer));
            }
        }
        // If a regular bullet is nearby, show the regular circle prefab
        if (bulletNearby)
        {
            if (regularCircle == null)
            {
                // Instantiate the regular circle
                regularCircle = Instantiate(circlePrefab, transform.position, Quaternion.identity);
                regularCircle.transform.SetParent(transform);  // Keep the circle relative to the player

                // Get the SpriteRenderer component of the regular circle
                regularCircleRenderer = regularCircle.GetComponent<SpriteRenderer>();

                // Start fading in the circle
                if (!isFading)
                    StartCoroutine(FadeInCircle(regularCircleRenderer));
            }
        }

        // If no bullet is nearby, fade out the circles
        if (!bulletNearby && !parryableBulletNearby)
        {
            if (regularCircle != null && !isFading)
                StartCoroutine(FadeOutCircle(regularCircle));

            if (parryableCircle != null && !isFading)
                StartCoroutine(FadeOutCircle(parryableCircle));
        }

        // Update the position of the circles to follow the player
        if (regularCircle != null)
        {
            regularCircle.transform.position = transform.position;
        }

        if (parryableCircle != null)
        {
            parryableCircle.transform.position = transform.position;
        }
    }

    // Coroutine to fade the circle in
    private IEnumerator FadeInCircle(SpriteRenderer circleRenderer)
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
    private IEnumerator FadeOutCircle(GameObject circle)
    {
        SpriteRenderer circleRenderer = circle.GetComponent<SpriteRenderer>();
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
        Destroy(circle);  // Destroy the circle once it is fully transparent
        isFading = false;
    }
}