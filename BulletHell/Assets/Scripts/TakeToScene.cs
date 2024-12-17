using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene loading
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    // Public float that can be set in the Inspector for delay
    public float delayBeforeChange = 2f;

    // The name of the scene you want to load
    public string sceneToLoad;

    // This function is called when another collider enters the trigger collider attached to this GameObject
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player is the one colliding with the object
        if (other.CompareTag("Player"))
        {
            // Start the scene change with delay
            StartCoroutine(ChangeSceneWithDelay());
        }
    }

    // Coroutine to wait for the specified delay before changing the scene
    private IEnumerator ChangeSceneWithDelay()
    {
        // Wait for the delay time
        yield return new WaitForSeconds(delayBeforeChange);

        // Load the specified scene
        SceneManager.LoadScene(sceneToLoad);
    }
}