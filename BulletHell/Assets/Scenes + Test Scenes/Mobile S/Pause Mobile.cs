using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMobile : MonoBehaviour
{
    // Reference to the Canvas containing the pause menu UI elements
    public Canvas pauseMenuCanvas;

    // Start is called before the first frame update
    void Start()
    {
        // Initially hide the pause menu canvas
        pauseMenuCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Detect the Escape key for pausing the game (for PC)
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 1)
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0)
        {
            Resume();
        }
    }

    // Function to pause the game when called (for both mobile and PC)
    public void PauseGame()
    {
        // Show the pause menu canvas
        pauseMenuCanvas.enabled = true;
        // Pause the game by setting Time.timeScale to 0
        Time.timeScale = 0;
    }

    // Function to resume the game when called
    public void Resume()
    {
        // Hide the pause menu canvas
        pauseMenuCanvas.enabled = false;
        // Resume the game by resetting Time.timeScale to 1
        Time.timeScale = 1;
    }

    // Function to reload the current scene (for the reload button)
    public void Reload()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Function to load a different scene (for the return to menu button)
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Mobile button to trigger the pause function
    public void OnPauseButtonClicked()
    {
        PauseGame();
    }
}
