using UnityEngine;

public class FadeInOutOnClick : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private bool isFadingIn = false;
    private bool isFadingOut = false;
    private float fadeSpeed = 1f;

    void Start()
    {
        // Get the CanvasGroup component on the object
        canvasGroup = GetComponent<CanvasGroup>();

        // Ensure the CanvasGroup is initially fully visible (set to 1 alpha)
        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup component not found on this object.");
            return;
        }
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    void Update()
    {
        // Check for left mouse click
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            // Toggle fade in and out
            if (canvasGroup.alpha == 1f)
            {
                StartCoroutine(FadeOut());
            }
            else
            {
                StartCoroutine(FadeIn());
            }
        }
    }

    private System.Collections.IEnumerator FadeIn()
    {
        isFadingIn = true;
        isFadingOut = false;

        // Fade in the object
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        canvasGroup.alpha = 1f;
        isFadingIn = false;
    }

    private System.Collections.IEnumerator FadeOut()
    {
        isFadingOut = true;
        isFadingIn = false;

        // Fade out the object
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        isFadingOut = false;
    }
}
