using UnityEngine;

public class MoveUp : MonoBehaviour
{
    public float speed = 5f;  // The speed at which the object moves up

    // Update is called once per frame
    void Update()
    {
        // Move the object upwards by modifying its position
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
}