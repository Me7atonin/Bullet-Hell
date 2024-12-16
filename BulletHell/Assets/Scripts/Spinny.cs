using UnityEngine;

public class SpinObject : MonoBehaviour
{
    // Speed of rotation
    public float spinSpeed = 100f;

    // Update is called once per frame
    void Update()
    {
        // Rotate the object around its Z-axis (for 2D)
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
    }
}