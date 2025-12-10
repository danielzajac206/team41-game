using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] int health;
    [SerializeField] int maxHealth = 5;
    [SerializeField] HealthScript healthScript;

    [SerializeField] float walkSpeed = 2.0f;
    [SerializeField] private Rigidbody2D playerBody;
    [SerializeField] private Vector2 playerVelocity;
    [SerializeField] private Animator animator;
    [SerializeField] GameObject hitboxPrefab;
    [SerializeField] GameObject fireballPrefab;
    [SerializeField] GameObject bowPrefab;

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
    //private List<KeyCode> keyStack = new List<KeyCode>();
    private string lastDir;
    private bool isAttacking = false;
    [SerializeField] private bool attackBuffered = false;
    //private float attackDuration = 0.55f;
    private Vector2 attackDirection;

    [SerializeField] private int comboStep = 0;
    [SerializeField] private float comboResetTime = 0.4f;
    private float comboTimer = 0f;

    public bool isLocked = false;

    private bool isKnockedback = false;
    private bool isDead = false;

    void Start()
    {
        inAction = false;
        animator = GetComponent<Animator>();
        healthScript = GameObject.Find("Health").GetComponent<HealthScript>();
        playerBody = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }

    private void FixedUpdate()
    {
        if (isKnockedback)
        {
            return;
        }
        //playerVelocity = new Vector2(x, y) * walkSpeed;
        playerBody.velocity = Vector2.ClampMagnitude(playerVelocity, walkSpeed);
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousePos - (Vector2)transform.position;

        playerVelocity.x = Input.GetAxisRaw("Horizontal") * walkSpeed;
        playerVelocity.y = Input.GetAxisRaw("Vertical") * walkSpeed;

        // Walking
        /*if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            playerBody.velocity = Vector2.ClampMagnitude(playerVelocity, walkSpeed);

        }*/

        // Allow diagonal movement
        /*if (Input.GetKeyDown(KeyCode.W)) { if (!keyStack.Contains(KeyCode.W)) keyStack.Add(KeyCode.W); }
        if (Input.GetKeyDown(KeyCode.S)) { if (!keyStack.Contains(KeyCode.S)) keyStack.Add(KeyCode.S); }
        if (Input.GetKeyDown(KeyCode.A)) { if (!keyStack.Contains(KeyCode.A)) keyStack.Add(KeyCode.A); }
        if (Input.GetKeyDown(KeyCode.D)) { if (!keyStack.Contains(KeyCode.D)) keyStack.Add(KeyCode.D); }

        if (Input.GetKeyUp(KeyCode.W)) { if (keyStack.Contains(KeyCode.W)) keyStack.Remove(KeyCode.W); }
        if (Input.GetKeyUp(KeyCode.S)) { if (keyStack.Contains(KeyCode.S)) keyStack.Remove(KeyCode.S); }
        if (Input.GetKeyUp(KeyCode.A)) { if (keyStack.Contains(KeyCode.A)) keyStack.Remove(KeyCode.A); }
        if (Input.GetKeyUp(KeyCode.D)) { if (keyStack.Contains(KeyCode.D)) keyStack.Remove(KeyCode.D); }

        if (keyStack.Count > 0)
        {
            switch (keyStack[keyStack.Count - 1])
            {
                case KeyCode.W: animator.Play("player-run-up"); break;
                case KeyCode.S: animator.Play("player-run-down"); break;
                case KeyCode.A: animator.Play("player-run-left"); break;
                case KeyCode.D: animator.Play("player-run-right"); break;
            }
        }*/

        if (isLocked)
        {
            playerBody.velocity = Vector2.zero;
            playerVelocity = Vector2.zero;
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                if (dir.x > 0)
                {
                    animator.Play("playeridle-right");
                }
                else
                {
                    animator.Play("playeridle-left");
                }
            }
            else
            {
                if (dir.y > 0)
                {
                    animator.Play("playeridle-up");
                }
                else
                {
                    animator.Play("playeridle-down");
                }
            }

            return;
        }

        if (comboStep > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer > comboResetTime && !isAttacking)
            {
                comboStep = 0;
                comboTimer = 0f;
            }
        }

        if (isAttacking)
        {
            playerBody.velocity = new Vector2(0, 0);
            playerVelocity = new Vector2(0, 0);
            //attackTimer -= Time.deltaTime;
            //if (attackTimer <= 0f) { 
            //    isAttacking = false;
            //}
            if (Input.GetMouseButtonDown(0))
            {
                attackBuffered = true;
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            lastDir = "x";
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            lastDir = "y";
        }

        if (playerVelocity.x != 0 && playerVelocity.y != 0)
        {
            if (lastDir == "x")
                playerVelocity.y = 0;
            else
                playerVelocity.x = 0;
        }

        if (playerVelocity.x < 0f)
        {
            animator.Play("player-run-left");
        }
        else if (playerVelocity.x > 0f)
        {
            animator.Play("player-run-right");
        }
        else if (playerVelocity.y > 0f)
        {
            animator.Play("player-run-up");
        }
        else if (playerVelocity.y < 0f)
        {
            animator.Play("player-run-down");
        }
        else
        {
            //playerBody.velocity = new Vector2(0, 0);

            // idle animations
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                if (dir.x > 0)
                {
                    animator.Play("playeridle-right");
                }
                else
                {
                    animator.Play("playeridle-left");
                }
            }
            else
            {
                if (dir.y > 0)
                {
                    animator.Play("playeridle-up");
                }
                else
                {
                    animator.Play("playeridle-down");
                }
            }
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

        // MAY NEED TO CHANGE???????????????
        foreach(Collider2D enemyCollider in hitEnemies)
        {
            //StartCoroutine(AttackMelee(dir));
            //Debug.Log("Attack");
            if (!isAttacking)
            {
                comboStep = 1;  // Start combo
                AttackMelee(dir);
            }
            else
            {
                // Buffer next hit
                attackBuffered = true;
            }
        }

        // shooting bow 
        else if (Input.GetMouseButtonDown(1))
        {
            //ShootProjectile(dir);
            //Debug.Log("Fireball");
            //Time.timeScale = 0.25f;
            Vector3 pos = transform.position;
            pos.z = -0.02f;
            GameObject bow = Instantiate(bowPrefab, pos, transform.rotation, transform);
            bow.transform.localScale = new Vector3(1f/transform.lossyScale.x, 1f / transform.lossyScale.y, 1f / transform.lossyScale.z);
            //animator.speed = 0f;
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

    private void AttackMelee(Vector2 dir)
    {
        isAttacking = true;
        attackBuffered = false;
        attackDirection = dir;

        if (comboStep == 1)
        {
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                if (dir.x > 0)
                {
                    animator.Play("player-attack1-right");
                }
                else
                {
                    animator.Play("player-attack1-left");
                }
            }
            else
            {
                if (dir.y > 0)
                {
                    animator.Play("player-attack1-up");
                }
                else
                {
                    animator.Play("player-attack1-down");
                }
            }
        }
        else
        {
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                if (dir.x > 0)
                {
                    animator.Play("player-attack2-right");
                }
                else
                {
                    animator.Play("player-attack2-left");
                }
            }
            else
            {
                if (dir.y > 0)
                {
                    animator.Play("player-attack2-up");
                }
                else
                {
                    animator.Play("player-attack2-down");
                }
            }
        }
    }

    public void Combo()
    {
        if (attackBuffered && comboStep == 1)
        {
            comboStep = 2;
            AttackMelee(attackDirection);
        }
    }

    public void AttackEnd()
    {
        if (comboStep == 2)
        {
            comboStep = 0;
            isAttacking = false;
            return;
        }

        if (comboStep == 1 && !attackBuffered)
        {
            comboStep = 0;
        }

        isAttacking = false;
    }

    //IEnumerator AttackMelee(Vector2 dir)
    //{
    //    isAttacking = true;
    //    attackBuffered = false;

    //    attackDirection = dir;
    //    comboTimer = 0f;
    //    if (comboStep == 1)
    //    {
    //        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
    //        {
    //            if (dir.x > 0)
    //            {
    //                animator.Play("player-attack1-right");
    //            }
    //            else
    //            {
    //                animator.Play("player-attack1-left");
    //            }
    //        }
    //        else
    //        {
    //            if (dir.y > 0)
    //            {
    //                animator.Play("player-attack1-up");
    //            }
    //            else
    //            {
    //                animator.Play("player-attack1-down");
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
    //        {
    //            if (dir.x > 0)
    //            {
    //                animator.Play("player-attack2-right");
    //            }
    //            else
    //            {
    //                animator.Play("player-attack2-left");
    //            }
    //        }
    //        else
    //        {
    //            if (dir.y > 0)
    //            {
    //                animator.Play("player-attack2-up");
    //            }
    //            else
    //            {
    //                animator.Play("player-attack2-down");
    //            }
    //        }
    //    }
    //    float timer = 0f;
    //    while (timer < attackDuration)
    //    {
    //        timer += Time.deltaTime;
    //        yield return null;
    //    }
    //    isAttacking = false;
    //    if (attackBuffered && comboStep == 1)
    //    {
    //        comboStep = 2;
    //        comboTimer = 0f;
    //        StartCoroutine(AttackMelee(dir));
    //    }
    //    else
    //    {
    //        if (comboStep == 2)
    //        {
    //            comboStep = 0;
    //        }
    //    }
    //}

    private void ShootProjectile(Vector2 dir)
    {
        Vector3 spawnPos = transform.position;
        spawnPos.z = -0.01f;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            spawnPos.x += dir.x > 0 ? 2 : -2;
        }
        else
        {
            spawnPos.y += dir.y > 0 ? 2 : -2;
        }

        GameObject fb = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            fb.transform.rotation = Quaternion.Euler(0, 0, dir.x > 0 ? 0 : 180);
        }
        else
        {
            fb.transform.rotation = Quaternion.Euler(0, 0, dir.y > 0 ? 90 : -90);
        }

        Rigidbody2D fireballRB = fb.GetComponent<Rigidbody2D>();
        if (fireballRB != null)
        {
            fireballRB.velocity = dir.normalized * fb.GetComponent<FireballScript>().speed;
        }
    }

    public void SpawnHitbox()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.z = -0.01f;

        if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
        {
            spawnPos.x += attackDirection.x > 0 ? 1.6f : -1.6f;
        }
        else
        {
            spawnPos.y += attackDirection.y > 0 ? 1.6f : -1.6f;
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

    public void TakeDamage(int damage, Vector2 direction, float force)
    {
        if (isDead) { return; }
        health -= damage;

        StartCoroutine(Knockback(direction, force, 0.1f));
        if (health <= 0)
        {
            health = 0;
            isDead = true;
            playerBody.velocity = Vector3.zero;
            Death();
        }
        healthScript.UpdateHealth(health);
    }

    IEnumerator Knockback(Vector2 direction, float force, float duration)
    {
        isKnockedback = true;

        playerBody.velocity = Vector2.zero;
        float timer = 0f;
        while (timer < duration)
        {
            playerBody.velocity = direction.normalized * force;
            timer += Time.deltaTime;
            yield return null;
        }

        playerBody.velocity = Vector2.zero;
        isKnockedback = false;
    }

    void Death()
    {
        playerBody.bodyType = RigidbodyType2D.Static;
        animator.Play("player-death");
        Time.timeScale = 0.25f;
        StartCoroutine(DeathAnim());
    }

    IEnumerator DeathAnim()
    {
        yield return new WaitForSecondsRealtime(4f);
        Time.timeScale = 0f;
    }
}