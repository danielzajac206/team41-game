using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] int maxHealth = 100;
    private int currentHealth;

    [Header("Movement Settings")]
    [SerializeField] float walkSpeed = 4.0f;
    [SerializeField] private Rigidbody2D playerBody;
    
    [Header("Combat Settings")]
    [SerializeField] float attackRange = 1.5f;    
    [SerializeField] int attackDamage = 25;       
    [SerializeField] float knockbackForce = 10f; 
    [SerializeField] LayerMask enemyLayers;       
    [SerializeField] float attackCooldown = 0.5f; 

    [Header("Interaction Settings")]
    [SerializeField] LayerMask interactionLayers; 

    [Header("Visuals")]
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite hitSprite;

    private Vector2 playerVelocity;
    private bool inAction;
    private float nextAttackTime = 0f;
    private bool isKnockedBack;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        inAction = false;
        currentHealth = maxHealth;
        
        if (playerBody == null) playerBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        playerBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        if (normalSprite == null && spriteRenderer != null)
        {
            normalSprite = spriteRenderer.sprite;
        }

        // Auto-fix: If interaction layers are set to "Nothing", default to "Everything"
        if (interactionLayers.value == 0)
        {
            interactionLayers = ~0; 
        }
    }

    void Update()
    {
        HandleMovement();
        HandleCombat();
    }

    private void HandleMovement()
    {
        if (isKnockedBack) return;

        playerVelocity.x = Input.GetAxis("Horizontal") * walkSpeed;
        playerVelocity.y = Input.GetAxis("Vertical") * walkSpeed;

        if (playerVelocity.magnitude > 0)
        {
            playerBody.velocity = Vector2.ClampMagnitude(playerVelocity, walkSpeed);
        } 
        else
        {
            playerBody.velocity = Vector2.zero;
        }
    }

    private void HandleCombat()
    {
        if (isKnockedBack) return; 

        if (!inAction && Input.GetKeyDown(KeyCode.Space)) Interact();
        if (Input.GetKeyDown(KeyCode.LeftShift)) Dodge();

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Interact();
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(PlayHitEffect());
        Debug.Log($"Player HP: {currentHealth}");
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        isKnockedBack = true;
        playerBody.velocity = direction * force; 
        StartCoroutine(ResetKnockback());
    }

    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(0.2f); 
        playerBody.velocity = Vector2.zero;    
        isKnockedBack = false;                 
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

    private void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayers);

        foreach(Collider2D enemyCollider in hitEnemies)
        {
            EnemyAI enemyScript = enemyCollider.GetComponentInParent<EnemyAI>();
            
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage);
                Vector2 knockbackDir = (enemyCollider.transform.position - transform.position).normalized;
                enemyScript.ApplyKnockback(knockbackDir, knockbackForce);
            }
        }
    }

    private void Interact()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, attackRange, interactionLayers);

        bool foundAnything = false;

        foreach(Collider2D obj in hitObjects)
        {
            GreenChargeButton chargeButton = obj.GetComponentInParent<GreenChargeButton>();
            if (chargeButton != null)
            {
                chargeButton.Interact();
                foundAnything = true;
                return;
            }

            ColorSwitch colorSwitch = obj.GetComponentInParent<ColorSwitch>();
            if (colorSwitch != null)
            {
                colorSwitch.Interact();
                foundAnything = true;
                return; 
            }

            GreenButton greenButton = obj.GetComponentInParent<GreenButton>();
            if (greenButton != null)
            {
                greenButton.Interact();
                foundAnything = true;
                return;
            }

            EnemyAI enemyScript = obj.GetComponentInParent<EnemyAI>();
            if (enemyScript != null)
            {
                Vector2 pushDir = (obj.transform.position - transform.position).normalized;
                enemyScript.ApplyKnockback(pushDir, 2f); 
                foundAnything = true;
            }
        }
    }

    private void Dodge() { }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}