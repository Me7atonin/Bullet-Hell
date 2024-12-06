using UnityEngine;

public class UpDownMovement : MonoBehaviour
{
    // Speed of movement
    public float speed = 2f;
    // Distance to move up and down
    public float distance = 3f;

    // Store the starting position
    private Vector3 startPosition;

    void Start()
    {
        // Save the initial position of the object
        startPosition = transform.position;
    }

    void Update()
    {
        // Move the object up and down based on time
        float newY = Mathf.Sin(Time.time * speed) * distance;

        // Update the object's position
        transform.position = new Vector3(startPosition.x, startPosition.y + newY, startPosition.z);
    }
}