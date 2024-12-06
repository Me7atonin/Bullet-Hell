using UnityEngine;

public class FollowPlayerYAxis : MonoBehaviour
{
    // Reference to the player's transform
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player not assigned to FollowPlayerYAxis script.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            // Follow only the player's Y position, keeping the X and Z positions fixed
            transform.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }
    }
}