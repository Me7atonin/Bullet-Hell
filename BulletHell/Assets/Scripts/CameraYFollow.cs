using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Reference to the player object
    public Transform player;

    // Optional offset to set the camera's starting position relative to the player
    public Vector3 offset;

    void Start()
    {
        // If no player is assigned in the Inspector, find the player by tag
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        // Initialize the offset if it's not assigned
        if (offset == Vector3.zero)
        {
            offset = transform.position - player.position;
        }
    }

    void Update()
    {
        // Keep the camera's X and Z fixed but update the Y position to follow the player
        Vector3 newPosition = new Vector3(transform.position.x, player.position.y + offset.y, transform.position.z);
        transform.position = newPosition;
    }
}
