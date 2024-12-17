using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class win : MonoBehaviour
{
    [SerializeField]
    string leveltoload = "Win";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        //IF player touches Dorito, load scene win
        if(collision.gameObject.tag == "Win" )
        {


            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //IF player touches Dorito, load Win scene
            SceneManager.LoadScene("Win");
            
        }
    }
}
