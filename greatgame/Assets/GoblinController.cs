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

    protected Animator animator;
    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;


    protected bool isAttacking = false;
    protected float attackTimer = 0f;
    protected float cooldownTimer = 0f;
    protected Vector2 attackDirection;
    protected bool isDead = false;

    public GameObject hitboxPrefab;

    public float viewRadius = 10f;
    public float viewAngle = 90f;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    protected bool isKnockedback = false;
    protected bool takingDamage = false;

    protected void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player").transform;
        health = maxHealth;
    }

    protected void Update()
    {
        if (isDead)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 dir = player.position - transform.position;
        float distance = dir.magnitude;

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
        else
        {
            if (cooldownTimer > 0f)
                cooldownTimer -= Time.deltaTime;
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
            //Move(dir);
            CheckVision();
        }
    }

    protected virtual void CheckVision()
    {
        if (isKnockedback) { return; }
        Collider2D playerInRange = Physics2D.OverlapCircle(transform.position, viewRadius, playerLayer);
        if (playerInRange != null)
        {
            Vector2 dir = (playerInRange.transform.position - transform.position).normalized;
            Vector2 forward;
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                forward = dir.x > 0 ? transform.right : -transform.right;
            }
            else
            {
                forward = dir.y > 0 ? transform.up : -transform.up;
            }
            float angleBetween = Vector2.Angle(forward, dir);

            if (angleBetween < viewAngle / 2)
            {
                //RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewRadius, obstacleLayer);
                //if (hit.collider == null)
                //{
                //    Debug.Log("player spotted");
                //    Move(dir);
                //}
                //else
                //{
                //    Debug.Log("view blocked by" + hit.collider.name);
                //}
                Move(dir);
            }
        }
        else if (takingDamage) {
            Vector2 dir = (player.transform.position - transform.position).normalized;
            Move(dir);
        }
        else
        {
            rb.velocity = Vector2.zero;
            animator.SetTrigger("Idle");
        }
    }


    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector2 dir = (player.position - transform.position).normalized;
        Vector2 forward;
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            forward = dir.x > 0 ? transform.right : -transform.right;
        }
        else
        {
            forward = dir.y > 0 ? transform.up : -transform.up;
        }
        Vector3 leftDir = Quaternion.Euler(0, 0, -viewAngle / 2) * forward;
        Vector3 rightDir = Quaternion.Euler(0, 0, viewAngle / 2) * forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftDir * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightDir * viewRadius);
    }


    protected virtual void Move(Vector2 dir)
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

    protected virtual void Attack(Vector2 dir)
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

    public virtual void TakeDamage(float damage, Vector2 direction, float force)
    {
        if (isDead) return;
        health -= damage;
        takingDamage = true;
        StartCoroutine(SetTakingDamageFalse());

        StartCoroutine(Knockback(direction, force, 0.1f));
        if (health <= 0)
        {
            health = 0;
            isDead = true;
            rb.velocity = Vector3.zero;

            Vector2 dir = player.position - transform.position;
            DeathAnim(dir);
        }
    }

    IEnumerator SetTakingDamageFalse()
    {
        if (!takingDamage)
        {
            yield break;
        }
        yield return new WaitForSeconds(5);
        takingDamage = false;
    }

    protected IEnumerator Knockback(Vector2 direction, float force, float duration)
    {
        isKnockedback = true;

        float timer = 0f;
        while (timer < duration)
        {
            rb.velocity = direction.normalized * force;
            timer += Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector2.zero;
        isKnockedback = false;
    }

    protected virtual void DeathAnim(Vector2 dir)
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
    protected void DestroyObject()
    {
        Destroy(gameObject);
    }
}
