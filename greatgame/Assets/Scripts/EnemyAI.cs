using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] 
public class EnemyAI : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] int maxHealth = 100;
    [SerializeField] float moveSpeed = 1.5f;
    [SerializeField] float followRange = 5.0f; 
    
    [Header("Combat")]
    [SerializeField] int damageToPlayer = 10;
    // Increased default radius to 2.0f to account for physical collision buffering
    [SerializeField] float attackRadius = 2.0f;   
    [SerializeField] float knockbackForce = 5f;   
    [SerializeField] float attackCooldown = 1.0f; 

    [Header("Visuals")]
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite hitSprite;

    private int currentHealth;
    private Transform playerTarget;
    private PlayerController playerScript; 
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private bool isKnockedBack; 
    private float nextAttackTime;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (normalSprite == null && spriteRenderer != null)
        {
            normalSprite = spriteRenderer.sprite;
        }

        rb.gravityScale = 0f; 
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
            playerScript = playerObj.GetComponent<PlayerController>();
        }
    }

    void Update()
    {
        if (playerTarget == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);

        if (distanceToPlayer <= attackRadius && Time.time >= nextAttackTime)
        {
            AttackPlayer();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void FixedUpdate()
    {
        if (playerTarget != null && !isKnockedBack)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);

            if (distanceToPlayer <= followRange && distanceToPlayer > attackRadius * 0.9f)
            {
                Vector2 direction = (playerTarget.position - transform.position).normalized;
                Vector2 newPos = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
                rb.MovePosition(newPos);
            }
        }
    }

    private void AttackPlayer()
    {
        if (playerScript != null)
        {
            playerScript.TakeDamage(damageToPlayer);

            Vector2 pushDir = (playerTarget.position - transform.position).normalized;
            playerScript.ApplyKnockback(pushDir, knockbackForce);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        StartCoroutine(PlayHitEffect());
        Debug.Log(gameObject.name + " HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        isKnockedBack = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
        StartCoroutine(ResetKnockback());
    }

    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(0.2f);
        rb.velocity = Vector2.zero; 
        isKnockedBack = false; 
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator PlayHitEffect()
    {
        if (spriteRenderer != null)
        {
            if (hitSprite != null) spriteRenderer.sprite = hitSprite;
            spriteRenderer.color = Color.red;
            
            yield return new WaitForSeconds(0.1f);
            
            spriteRenderer.color = Color.white;
            if (normalSprite != null) spriteRenderer.sprite = normalSprite;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}