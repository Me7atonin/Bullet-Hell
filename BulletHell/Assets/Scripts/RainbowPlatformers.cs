using UnityEngine;

public class RainbowColor : MonoBehaviour
{
    // Public float to control the speed of the color change
    public float colorChangeSpeed = 1f;

    // Reference to the Renderer component
    private Renderer objectRenderer;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Renderer component of the object
        objectRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update the material color with a smooth transition through the rainbow
        objectRenderer.material.color = Color.HSVToRGB(Mathf.PingPong(Time.time * colorChangeSpeed, 1), 1, 1);
    }
}