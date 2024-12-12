using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //IF the enter/return key is pressed
        if(Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 1)
        {
        //display the pause menu
        GetComponent<Canvas>().enabled = true;
        //pause the game
        Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0)
        {
            Resume();
        }

    }
    public void Resume()
    {
        //hide the pause canvas again
        GetComponent<Canvas>().enabled = false;
        //reset the timescale to 1
        Time.timeScale = 1;
    }
    public void Reload()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
