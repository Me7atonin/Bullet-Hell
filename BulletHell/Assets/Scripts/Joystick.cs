using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour,IPointerUpHandler,IPointerDownHandler
{
    [SerializeField]
    private InputActionReference moveActionToUse;
    [SerializeField]
    private float moveSpeed = 10f;
    private InputAction moveAction;
    public float JumpSpeed = 5f;
    bool Grounded = false;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        moveAction = moveActionToUse.action; 
        moveAction.Enable(); 
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        moveInput.y = 0; 

        if (moveInput.magnitude > 1)
        {
            moveInput.Normalize();
        }

        
        transform.Translate(moveInput.x * moveSpeed * Time.deltaTime, 0, 0);

        
        Debug.Log("New Position: " + transform.position);

        if (Input.GetButtonDown("Jump") && Grounded) 
        {
            rb.AddForce(new Vector2(0, 100 * JumpSpeed));
            Grounded = false;
        }

    }

   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Grounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Grounded = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
