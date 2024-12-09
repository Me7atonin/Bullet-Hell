using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Joystick : MonoBehaviour
{
    [SerializeField]
    private InputActionReference moveActionToUse;
    [SerializeField]
    private float moveSpeed = 10f;
    private InputAction moveAction;

    // Start is called before the first frame update
    void Start()
    {
        moveAction = moveActionToUse.action; // Cache the reference to the InputAction
        moveAction.Enable(); // Ensure input action is enabled
    }

    // Update is called once per frame
    void Update()
    {
        // Get the movement vector from the input system
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        // Only use the horizontal (x) movement
        moveInput.y = 0; // Set vertical movement to 0

        // Optionally normalize if you want consistent movement speed
        if (moveInput.magnitude > 1)
        {
            moveInput.Normalize();
        }

        // Apply the movement using transform.Translate (directly moving the character)
        transform.Translate(moveInput.x * moveSpeed * Time.deltaTime, 0, 0);

        // Debug log to check position
        Debug.Log("New Position: " + transform.position);
    }
}
