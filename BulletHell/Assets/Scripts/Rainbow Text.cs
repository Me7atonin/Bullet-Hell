using UnityEngine;
using UnityEngine.UI;

public class RainbowTextEffect : MonoBehaviour
{
    public float speed = 1f;   // Speed at which the color changes
    private Text textComponent; // The Text component
    private float time;        // Time counter to cycle colors

    private void Start()
    {
        // Get the Text component attached to the GameObject
        textComponent = GetComponent<Text>();
    }

    private void Update()
    {
        // Cycle through the rainbow colors over time
        time += Time.deltaTime * speed;
        textComponent.color = Color.HSVToRGB(Mathf.PingPong(time, 1f), 1f, 1f);
    }
}
