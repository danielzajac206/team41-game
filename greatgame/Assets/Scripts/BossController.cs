using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 500;
    private int currentHealth;

    [Header("Attack Settings")]
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Sprite chargingSprite; // Crystal Knight_66 sprite
    [SerializeField] private Transform player;
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float fireballSpeed = 8f;
    [SerializeField] private float sideFireballSpread = 3f; // Distance to sides
    [SerializeField] private float laserDamage = 10f;
    [SerializeField] private float laserChargeTime = 1f;
    [SerializeField] private float laserDuration = 1.5f;
    
    private float nextAttackTime = 0f;
    private Animator animator;
    private bool isAttacking = false;
    private bool isDead = false;
    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
        nextAttackTime = Time.time + attackCooldown; // Initialize first attack time
    }

    void Update()
    {
        if (isDead) return; // Stop all logic when dead
        
        // DEBUG: Press 5 to instantly kill boss
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentHealth = -1;
            Die();
            return;
        }
        
        if (player != null && Time.time >= nextAttackTime && !isAttacking)
        {
            StartCoroutine(AttackSequence());
            nextAttackTime = Time.time + attackCooldown;
        }
        else if (!isAttacking && animator != null)
        {
            animator.CrossFade("boss_idle", 0);
        }
    }

    IEnumerator AttackSequence()
    {
        isAttacking = true;
        
        // Randomly choose between three attack types
        float rand = Random.value;
        
        if (rand < 0.33f)
        {
            // Three fireball attack
            animator.CrossFade("boss_shoot", 0);
            yield return new WaitForSeconds(0.3f);
            ShootThreeFireballs();
            yield return new WaitForSeconds(0.5f);
        }
        else if (rand < 0.66f)
        {
            // Laser attack
            yield return StartCoroutine(LaserAttack());
        }
        else
        {
            // Radial barrage attack
            yield return StartCoroutine(RadialBarrageAttack());
        }
        
        isAttacking = false;
        animator.CrossFade("boss_idle", 0);
    }

    void ShootThreeFireballs()
    {
        if (player == null)
        {
            return;
        }
        
        Vector2 playerPos2D = new Vector2(player.position.x, player.position.y);
        
        Vector2 directionToPlayer = (playerPos2D - (Vector2)transform.position).normalized;
        float angleToPlayer = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        
        // Calculate perpendicular direction for side targets
        Vector2 perpendicular = new Vector2(-directionToPlayer.y, directionToPlayer.x);

        // Center fireball - directly at player
        ShootFireball(playerPos2D, angleToPlayer);

        // Left and right fireballs - offset from player position
        Vector2 leftTarget = playerPos2D + (perpendicular * sideFireballSpread);
        Vector2 rightTarget = playerPos2D - (perpendicular * sideFireballSpread);
        
        Vector2 leftDir = (leftTarget - (Vector2)transform.position).normalized;
        Vector2 rightDir = (rightTarget - (Vector2)transform.position).normalized;
        
        float leftAngle = Mathf.Atan2(leftDir.y, leftDir.x) * Mathf.Rad2Deg;
        float rightAngle = Mathf.Atan2(rightDir.y, rightDir.x) * Mathf.Rad2Deg;
        
        ShootFireball(leftTarget, leftAngle);
        ShootFireball(rightTarget, rightAngle);
    }

    void ShootFireball(Vector2 targetPosition, float angle)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        
        // Spawn fireball offset from boss to avoid immediate collision
        Vector3 spawnPosition = transform.position + (Vector3)(direction * 1.5f);
        spawnPosition.z = -0.01f; // Ensure it's visible
        
        GameObject fireball = Instantiate(fireballPrefab, spawnPosition, Quaternion.Euler(0, 0, angle));
        
        // Ignore collision between boss and fireball
        Collider2D fireballCollider = fireball.GetComponent<Collider2D>();
        Collider2D bossCollider = GetComponent<Collider2D>();
        if (fireballCollider != null && bossCollider != null)
        {
            Physics2D.IgnoreCollision(fireballCollider, bossCollider);
        }
        
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * fireballSpeed;
        }
    }

    IEnumerator RadialBarrageAttack()
    {
        animator.CrossFade("boss_shoot", 0);
        yield return new WaitForSeconds(0.3f);
        
        // First barrage - 8 cardinal and ordinal directions
        float[] angles1 = { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };
        foreach (float angle in angles1)
        {
            ShootFireballInDirection(angle);
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // Second barrage - offset by 22.5 degrees (in between the first 8)
        float[] angles2 = { 22.5f, 67.5f, 112.5f, 157.5f, 202.5f, 247.5f, 292.5f, 337.5f };
        foreach (float angle in angles2)
        {
            ShootFireballInDirection(angle);
        }
        
        yield return new WaitForSeconds(0.5f);
    }

    void ShootFireballInDirection(float angle)
    {
        // Convert angle to radians for direction calculation
        float rad = angle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        
        // Spawn fireball offset from boss
        Vector3 spawnPosition = transform.position + (Vector3)(direction * 1.5f);
        spawnPosition.z = -0.01f;
        
        GameObject fireball = Instantiate(fireballPrefab, spawnPosition, Quaternion.Euler(0, 0, angle));
        
        // Ignore collision between boss and fireball
        Collider2D fireballCollider = fireball.GetComponent<Collider2D>();
        Collider2D bossCollider = GetComponent<Collider2D>();
        if (fireballCollider != null && bossCollider != null)
        {
            Physics2D.IgnoreCollision(fireballCollider, bossCollider);
        }
        
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * fireballSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return; // Don't take damage when dead
        
        // Only take damage from arrows/bow projectiles
        if (collision.gameObject.name.Contains("Arrow") || collision.gameObject.name.Contains("arrow"))
        {
            TakeDamage(25);
            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return; // Prevent multiple death calls
        isDead = true;
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        // Stop music
        MusicManager musicManager = FindObjectOfType<MusicManager>();
        if (musicManager != null)
        {
            musicManager.StopMusic();
        }
        
        // Play death animation
        if (animator != null)
        {
            animator.enabled = true; // Make sure animator is enabled
            animator.CrossFade("boss_death", 0);
        }
        
        // Wait for animation to complete (adjust time based on your animation length)
        yield return new WaitForSeconds(2f);
        
        // Destroy the boss
        Destroy(gameObject);
    }

    IEnumerator LaserAttack()
    {
        // Disable animator to allow manual sprite change
        if (animator != null)
        {
            animator.enabled = false;
        }
        
        // Change to charging sprite
        if (chargingSprite != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = chargingSprite;
        }
        
        // Charge up time
        yield return new WaitForSeconds(laserChargeTime);
        
        // Spawn laser at specified position
        Vector3 laserPos = new Vector3(0.61f, 47.04f, -0.01f);
        GameObject laser = Instantiate(laserPrefab, laserPos, Quaternion.identity);
        
        // Restore original sprite
        if (originalSprite != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = originalSprite;
        }
        
        // Re-enable animator
        if (animator != null)
        {
            animator.enabled = true;
        }
        
        // Wait for laser duration, then destroy
        yield return new WaitForSeconds(laserDuration);
        Destroy(laser);
    }
}

