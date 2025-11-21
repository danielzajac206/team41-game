using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float maxHealth = 100;

    public Transform player;
    public float moveSpeed = 2f;
    public float attackRange = 1.2f;
    public float attackDuration = 0.75f;
    public float attackCooldown = .5f;

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;


    private bool isAttacking = false;
    private float attackTimer = 0f;
    private float cooldownTimer = 0f;
    private Vector2 attackDirection;
    private bool isDead = false;

    public GameObject hitboxPrefab;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player").transform;
        health = maxHealth;
    }

    void Update()
    {
        if (isDead)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 dir = player.position - transform.position;
        float distance = dir.magnitude;

        if (attackTimer > 0f)
            attackTimer -= Time.deltaTime;
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                isAttacking = false;
            }
            rb.velocity = Vector2.zero;
            return;
        }

        if (distance <= attackRange)
        {
            if (cooldownTimer <= 0f)
            {
                Attack(dir);
                cooldownTimer = attackCooldown;
            }
        }
        else
        {
            Move(dir);
        }
    }

    void Move(Vector2 dir)
    {
        Vector2 moveDir = dir.normalized;
        rb.velocity = moveDir * moveSpeed;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0)
            {
                spriteRenderer.flipX = true;
                animator.Play("goblin_s_walk");
            }
            else
            {
                spriteRenderer.flipX = false;
                animator.Play("goblin_s_walk");
            }
        }
        else
        {
            if (dir.y > 0)
                animator.Play("goblin_u_walk");
            else
                animator.Play("goblin_d_walk");
        }
    }

    void Attack(Vector2 dir)
    {
        isAttacking = true;
        attackTimer = attackDuration;
        rb.velocity = Vector2.zero;

        attackDirection = dir;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0)
            {
                spriteRenderer.flipX = true;
                animator.Play("goblin_s_attack");
            }
            else
            {
                spriteRenderer.flipX = false;
                animator.Play("goblin_s_attack");
            }
        }
        else
        {
            if (dir.y > 0)
            {
                animator.Play("goblin_u_attack");
            }
            else
            {
                animator.Play("goblin_d_attack");
            }
        }
    }

    public void SpawnHitbox()
    {
        Vector3 spawnPos = transform.position;

        if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
        {
            spawnPos.x += attackDirection.x > 0 ? attackRange / 2 : -attackRange / 2;
        }
        else
        {
            spawnPos.y += attackDirection.y > 0 ? attackRange / 2 : -attackRange / 2;
        }

        GameObject hb = Instantiate(hitboxPrefab, spawnPos, Quaternion.identity);

        if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
        {
            hb.transform.rotation = Quaternion.Euler(0, 0, attackDirection.x > 0 ? 0 : 180);
        }
        else
        {
            hb.transform.rotation = Quaternion.Euler(0, 0, attackDirection.y > 0 ? 90 : -90);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            isDead = true;
            rb.velocity = Vector3.zero;

            Vector2 dir = player.position - transform.position;
            DeathAnim(dir);
        }
    }

    void DeathAnim(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0)
            {
                spriteRenderer.flipX = true;
                animator.Play("goblin_s_death");
            }
            else
            {
                spriteRenderer.flipX = false;
                animator.Play("goblin_s_death");
            }
        }
        else
        {
            if (dir.y > 0)
            {
                animator.Play("goblin_u_death");
            }
            else
            {
                animator.Play("goblin_d_death");
            }
        }
    }
    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
