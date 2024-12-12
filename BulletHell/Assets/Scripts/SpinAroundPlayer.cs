using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockSpinAroundPlayer : MonoBehaviour
{
    public Transform player;          // Reference to the player object
    public float spinSpeed = 50f;     // Rotation speed (degrees per second)
    public float radius = 2f;         // Distance from the player (radius)

    private Vector3 offset;           // Initial offset between the object and the player
    private float angle;              // The current angle of the rotation

    void Start()
    {
        // Calculate initial offset from the player
        if (player != null)
        {
            offset = transform.position - player.position;
            // Set the angle to the initial angle based on the starting position
            angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Update the angle of rotation based on spin speed and deltaTime
            angle += spinSpeed * Time.deltaTime;

            // Ensure the angle stays between 0 and 360 degrees
            if (angle >= 360f) angle -= 360f;

            // Calculate the new position of the object based on the angle and radius
            float x = player.position.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = player.position.y + radius * Mathf.Sin(angle * Mathf.Deg2Rad);

            // Update the object's position, but lock the Z-axis to the player's Z position
            transform.position = new Vector3(x, y, player.position.z);
        }
    }
}