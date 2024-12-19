using UnityEngine;

public class SpinAttack : MonoBehaviour
{
    public float spinDuration = 1f;  // Duration for the spin (in seconds)
    private float spinTimer = 0f;
    private bool isSpinning = true;

    void Update()
    {
        // Spin the object 180 degrees over the duration of spinDuration
        if (isSpinning)
        {
            spinTimer += Time.deltaTime;
            float rotationAmount = Mathf.Lerp(0f, 180f, spinTimer / spinDuration);  // Lerp between 0 and 180 degrees
            transform.localRotation = Quaternion.Euler(0, 0, rotationAmount);  // Rotate around the Z axis locally
        }

        // If the spinning is done, stop and remove the spinning behavior
        if (spinTimer >= spinDuration)
        {
            isSpinning = false;
            Destroy(gameObject, 0.1f);  // Destroy the object after a short delay
        }
    }
}