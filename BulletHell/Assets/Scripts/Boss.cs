using System.Collections;  // Ensure this is included
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float moveSpeed = 3f;  // Speed at which the enemy moves
    public float reducedSpeedFactor = 0.1f;  // Reduced speed factor during attack (e.g., 0.1 for 10% speed)
    public float detectionRange = 10f;  // Range within which the enemy detects the player
    public float attackRange = 2f;  // Range at which the enemy will perform the spin attack
    public Transform player;  // Reference to the player's transform
    public GameObject spinPrefab;  // Prefab to spawn (the object that will spin)
    public float attackCooldown = 2f;  // Cooldown period after each attack
    public float pauseBeforeAttack = 1f;  // Time to wait before executing the spin attack
    public float spinRadius = 2f;  // Radius around the enemy where the prefab will rotate
    public float spinDuration = 1f;  // Duration for the 180-degree spin

    [Header("Platform Detection")]
    public float groundCheckDistance = 1f;  // Distance to check for ground (platform detection)
    public LayerMask groundLayer;  // Ground layer to detect edges of platforms

    private Rigidbody2D rb;
    private bool isPlayerInRange = false;  // Whether the player is within detection range
    private bool isFacingRight = true;  // Tracks if the enemy is facing right or left
    private bool canAttack = true;  // Flag to check if the enemy can attack (after cooldown)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Get Rigidbody2D component
        if (player == null)
        {
            // Automatically find the player if not assigned
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        DetectPlayer();
        if (isPlayerInRange)
        {
            ChasePlayer();
            TrySpinAttack();
        }
        CheckPlatformEdge();
    }

    // Detect if the player is within range
    void DetectPlayer()
    {
        if (player != null && Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            isPlayerInRange = true;
        }
        else
        {
            isPlayerInRange = false;
        }
    }

    // Move the enemy toward the player
    void ChasePlayer()
    {
        // Only chase the player if the enemy is not preparing to attack
        if (canAttack)
        {
            MoveAtNormalSpeed();
        }
        else
        {
            MoveAtReducedSpeed();
        }
    }

    // Normal movement speed (when not attacking)
    void MoveAtNormalSpeed()
    {
        Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);  // Keep enemy on same vertical level
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;  // Convert transform.position to Vector2 for correct subtraction

        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);  // Move horizontally, maintain vertical velocity
    }

    // Reduced movement speed during attack (using the reducedSpeedFactor)
    void MoveAtReducedSpeed()
    {
        Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed * reducedSpeedFactor, rb.velocity.y);  // Reduced speed during attack
    }

    // Flip the enemy's direction when needed
    void Flip()
    {
        if ((isFacingRight && transform.position.x > player.position.x) || (!isFacingRight && transform.position.x < player.position.x))
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;  // Flip the enemy's sprite
            transform.localScale = scale;
        }
    }

    // Check if the enemy is about to fall off the platform and flip if necessary
    void CheckPlatformEdge()
    {
        if (IsGrounded() && !IsGroundedInDirection())
        {
            Flip();
        }
    }

    // Check if the enemy is grounded (i.e., standing on the platform)
    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        return hit.collider != null;
    }

    // Check if the ground is detected in the direction the enemy is facing
    bool IsGroundedInDirection()
    {
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, groundCheckDistance, groundLayer);
        return hit.collider != null;
    }

    // Try to perform a spin attack if the player is close enough
    void TrySpinAttack()
    {
        // Check if the player is within the attack range and if the enemy is allowed to attack
        if (Vector2.Distance(transform.position, player.position) <= attackRange && canAttack)
        {
            StartCoroutine(WaitBeforeAttack());  // Start the wait before attack coroutine
        }
    }

    // Coroutine to handle the delay before the spin attack
    IEnumerator WaitBeforeAttack()
    {
        canAttack = false;  // Disable further attacks during cooldown

        // Move the enemy at reduced speed during the attack
        float originalSpeed = moveSpeed;  // Store the original speed
        moveSpeed *= reducedSpeedFactor;  // Set move speed to the reduced speed (based on reducedSpeedFactor)

        // Wait for the specified amount of time before attacking
        yield return new WaitForSeconds(pauseBeforeAttack);  // Pause before attacking

        // After the delay, spawn the spin prefab and perform the attack
        SpawnSpinPrefab();

        // Wait for the cooldown before enabling another attack
        yield return new WaitForSeconds(attackCooldown);

        moveSpeed = originalSpeed;  // Restore the original speed
        canAttack = true;  // Re-enable attacks after cooldown
    }

    // Spawn the spinning prefab at the enemy's position and make it spin
    void SpawnSpinPrefab()
    {
        // Determine the spawn position, offset slightly in front of the enemy
        Vector3 spawnPosition = transform.position + (isFacingRight ? Vector3.right : Vector3.left) * spinRadius;
        GameObject spinObject = Instantiate(spinPrefab, spawnPosition, Quaternion.identity);

        // Make the prefab rotate 180 degrees around the enemy
        StartCoroutine(RotateAroundEnemy(spinObject));
    }

    // Coroutine to rotate the prefab 180 degrees around the enemy
    IEnumerator RotateAroundEnemy(GameObject spinObject)
    {
        float elapsedTime = 0f;

        // The center of rotation is the enemy's position
        Vector3 rotationCenter = transform.position;

        // Determine the direction for rotation
        float angle = 0f;
        float endAngle = 180f * (isFacingRight ? 1 : -1);  // Rotate clockwise or counterclockwise depending on facing direction

        // Rotate the prefab smoothly over the specified duration
        while (elapsedTime < spinDuration)
        {
            // Calculate the current angle based on the elapsed time and duration
            angle = Mathf.Lerp(0f, endAngle, elapsedTime / spinDuration);

            // Rotate the prefab around the enemy
            spinObject.transform.position = rotationCenter + (Quaternion.Euler(0f, 0f, angle) * Vector3.right * spinRadius);

            elapsedTime += Time.deltaTime;  // Increase elapsed time by the frame duration
            yield return null;
        }

        // Ensure the prefab ends up at the exact final position after rotation
        spinObject.transform.position = rotationCenter + (Quaternion.Euler(0f, 0f, endAngle) * Vector3.right * spinRadius);
    }
}
