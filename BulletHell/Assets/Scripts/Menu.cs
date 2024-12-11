using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    string leveltoload = "1stPart";
    // Start is called before the first frame update
  

    public void LoadNextInBuild()
        {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    public void QuitGame()
    {
        Application.Quit();
    }

}
