using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float moveSpeed = 5f; // Normal movement speed
    public float chaseSpeed = 8f; // Speed when chasing the player
    public float jumpForce = 15f; // Jump force for the enemy
    public float attackIntervalMin = 3f; // Minimum time between attacks
    public float attackIntervalMax = 6f; // Maximum time between attacks
    public GameObject bulletPrefab; // Bullet prefab for shooting
    public Transform shootPoint; // Point from which the bullets will be shot
    public LayerMask groundLayer; // Ground detection layer
    public Transform player; // Reference to the player object
    public float slamSpeed = 30f; // Speed for slam attack
    public float spinAttackDuration = 3f; // Duration for spin attack
    public float spinAttackSpeed = 100f; // Speed for spinning bullets
    public float edgeDetectionDistance = 1f; // Distance to detect the edge
    public LayerMask edgeLayer; // Layer for detecting edges (e.g., walls)
    public float stopTime = 2f; // Time to stop before choosing an attack
    public float bulletCount = 5f; // Number of bullets to shoot in attack 3 and 4
    public float slamAttackDuration = 10f; // Duration for the Slam Attack

    // Public Floats for controlling attack availability
    public float slamAttackEnabled = 1f; // 0 = disabled, 1 = enabled
    public float runAttackEnabled = 1f; // 0 = disabled, 1 = enabled
    public float spinAttackEnabled = 1f; // 0 = disabled, 1 = enabled
    public float shootAtPlayerEnabled = 1f; // 0 = disabled, 1 = enabled
    public float growthAttackEnabled = 1f; // 0 = disabled, 1 = enabled

    private Rigidbody2D rb;
    private bool isChasing = true;
    private bool isPerformingAttack = false;
    private float nextAttackTime;
    private Vector2 lastPlayerPosition;
    private float attackCooldown;
    private bool isMovingRight = true; // Track current direction for RunAttack
    private float stopTimer = 0f; // Timer to control stop time
    private float attackTimer = 0f; // Timer to control the attack duration
    private float originalScaleX;

    private enum AttackType
    {
        None,
        SlamAttack,
        RunAttack,
        SpinAttack,
        ShootAtPlayer,
        GrowthAndRotationAttack
    }

    private AttackType currentAttack = AttackType.None;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScaleX = transform.localScale.x; // Store the original scale
        nextAttackTime = Random.Range(attackIntervalMin, attackIntervalMax);
        lastPlayerPosition = player.position;
    }

    private void Update()
    {
        if (!isPerformingAttack)
        {
            if (isChasing)
            {
                ChasePlayer();
            }

            if (stopTimer > 0)
            {
                stopTimer -= Time.deltaTime; // Decrease the stop timer
                return; // Wait until stopTimer finishes before proceeding
            }

            nextAttackTime -= Time.deltaTime;

            if (nextAttackTime <= 0)
            {
                // After the stop time, choose a random attack
                ChooseRandomAttack();
            }
        }
        else
        {
            PerformAttack();
        }
    }

    private void ChasePlayer()
    {
        if (player.position.x > transform.position.x)
        {
            // Move right towards the player
            rb.velocity = new Vector2(chaseSpeed, rb.velocity.y);
        }
        else
        {
            // Move left towards the player
            rb.velocity = new Vector2(-chaseSpeed, rb.velocity.y);
        }

        // If the player is above the enemy, make the enemy jump towards the player
        if (Mathf.Abs(player.position.y - transform.position.y) > 2f && IsGrounded())
        {
            JumpTowardsPlayer();
        }
    }

    private void JumpTowardsPlayer()
    {
        if (IsGrounded())
        {
            Vector2 jumpDirection = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(rb.velocity.x, 0); // Reset vertical velocity
            rb.AddForce(new Vector2(jumpDirection.x * 5f, jumpForce), ForceMode2D.Impulse);
        }
    }

    private bool IsGrounded()
    {
        // Check if the enemy is on the ground
        return Physics2D.Raycast(transform.position, Vector2.down, 0.5f, groundLayer);
    }

    private void ChooseRandomAttack()
    {
        // Stop the enemy for 2 seconds before choosing an attack
        stopTimer = stopTime;

        int attackIndex = Random.Range(0, 5);

        // Choose one of the five attack types randomly based on the enabled floats
        switch (attackIndex)
        {
            case 0:
                if (slamAttackEnabled == 1f) currentAttack = AttackType.SlamAttack;
                break;
            case 1:
                if (runAttackEnabled == 1f) currentAttack = AttackType.RunAttack;
                break;
            case 2:
                if (spinAttackEnabled == 1f) currentAttack = AttackType.SpinAttack;
                break;
            case 3:
                if (shootAtPlayerEnabled == 1f) currentAttack = AttackType.ShootAtPlayer;
                break;
            case 4:
                if (growthAttackEnabled == 1f) currentAttack = AttackType.GrowthAndRotationAttack;
                break;
        }

        isPerformingAttack = true;
        attackCooldown = 0;
        attackTimer = 0f; // Reset attack timer when a new attack starts
    }

    private void PerformAttack()
    {
        attackTimer += Time.deltaTime; // Increase the attack duration timer

        // Perform the selected attack
        switch (currentAttack)
        {
            case AttackType.SlamAttack:
                SlamAttack();
                break;
            case AttackType.RunAttack:
                RunAttack();
                break;
            case AttackType.SpinAttack:
                SpinAttack();
                break;
            case AttackType.ShootAtPlayer:
                ShootAtPlayer();
                break;
            case AttackType.GrowthAndRotationAttack:
                GrowthAndRotationAttack();
                break;
        }

        // Check if the attack duration has passed and end the attack if necessary
        if (attackTimer >= slamAttackDuration && currentAttack == AttackType.SlamAttack)
        {
            EndAttack();
        }
        else if (attackTimer >= 3f && currentAttack != AttackType.GrowthAndRotationAttack) // For other attacks, end after a shorter time
        {
            EndAttack();
        }
    }

    private void SlamAttack()
    {
        // Make sure the enemy is grounded before performing the slam attack
        if (IsGrounded() && attackTimer < slamAttackDuration)
        {
            // Reset vertical velocity before applying the slam force
            rb.velocity = new Vector2(rb.velocity.x, 0);

            // Apply a high force downwards to simulate the slam attack
            rb.AddForce(Vector2.down * slamSpeed, ForceMode2D.Impulse);
        }
    }

    private void RunAttack()
    {
        // The enemy runs back and forth extremely quickly
        DetectEdgeAndMove();

        attackCooldown += Time.deltaTime;
        if (attackCooldown >= 2f) // Run for 2 seconds
        {
            EndAttack();
        }
    }

    private void DetectEdgeAndMove()
    {
        // Check for the edge in front of the enemy using raycasting
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * (isMovingRight ? 1 : -1), edgeDetectionDistance, edgeLayer);

        if (hit.collider == null)
        {
            // If no ground is detected in front, reverse direction
            isMovingRight = !isMovingRight;
        }

        // Move in the current direction
        rb.velocity = new Vector2((isMovingRight ? 1 : -1) * slamSpeed, rb.velocity.y);
    }

    private void SpinAttack()
    {
        // The enemy shoots bullets in a spinning pattern
        attackCooldown += Time.deltaTime;
        if (attackCooldown < spinAttackDuration)
        {
            // Shoot the number of bullets based on bulletCount
            float angleStep = 360f / bulletCount;
            for (float angle = 0f; angle < 360f; angle += angleStep)
            {
                ShootBulletAtAngle(angle);
            }
        }
        else
        {
            EndAttack();
        }
    }

    private void ShootAtPlayer()
    {
        // The enemy shoots bullets at the player's position
        attackCooldown += Time.deltaTime;
        if (attackCooldown < 2f)
        {
            // Shoot the number of bullets based on bulletCount
            for (int i = 0; i < bulletCount; i++)
            {
                ShootBulletAtPlayer();
            }
        }
        else
        {
            EndAttack();
        }
    }

    private void GrowthAndRotationAttack()
    {
        // The enemy stops, grows 3 times its size, and starts rotating

        // Scale up the enemy
        if (attackTimer < 3f)
        {
            transform.localScale = new Vector3(originalScaleX * 3f, originalScaleX * 3f, 1f);
            transform.Rotate(0f, 0f, 50f * Time.deltaTime); // Rotate the enemy continuously
        }
        else
        {
            // After 3 seconds, return to the original size
            transform.localScale = new Vector3(originalScaleX, originalScaleX, 1f);
            EndAttack();
        }
    }

    private void ShootBulletAtAngle(float angle)
    {
        // Shoot a bullet at the given angle
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        bullet.transform.Rotate(0f, 0f, angle);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = bullet.transform.right * 10f; // Shoot in the direction of the angle
    }

    private void ShootBulletAtPlayer()
    {
        // Shoot a bullet directly at the player
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        Vector2 directionToPlayer = (player.position - shootPoint.position).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = directionToPlayer * 10f;
    }

    private void EndAttack()
    {
        // Reset after performing the attack
        currentAttack = AttackType.None;
        isPerformingAttack = false;
        nextAttackTime = Random.Range(attackIntervalMin, attackIntervalMax); // Set a new random interval before the next attack
    }
}
