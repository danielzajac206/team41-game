using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] int health;
    [SerializeField] int maxHealth = 10;
    [SerializeField] HealthScript healthScript;
    private int numKeys = 0;

    [Header("Movement Settings")]
    [SerializeField] float walkSpeed = 5.0f;
    [SerializeField] private Rigidbody2D playerBody;
    [SerializeField] private Vector2 playerVelocity;
    private bool inAction;

    [Header("Dodge Settings")]
    [SerializeField] private float dodgeSpeed = 12f;
    [SerializeField] private float dodgeDuration = 0.25f;
    [SerializeField] private float dodgeCooldown = 0.6f;
    private float dodgeCooldownTimer = 0f;

    private bool isDodging = false;

    [Header("Combat Settings")]  
    [SerializeField] int attackDamage = 50;       
    [SerializeField] float knockbackForce = 10f; 
    [SerializeField] LayerMask enemyLayers;       
    [SerializeField] float attackCooldown = 0.5f;

    [SerializeField] private int comboStep = 0;
    [SerializeField] private float comboResetTime = 0.4f;
    private float comboTimer = 0f;
    public bool isLocked = false;
    private bool isKnockedback = false;
    private bool isDead = false;
    private bool isAttacking = false;
    [SerializeField] private bool attackBuffered = false;
    private Vector2 attackDirection;
    [SerializeField] public float bowCooldown = 1f;
    public float bowCooldownTimer = 0f;

    [SerializeField] GameObject hitboxPrefab;
    [SerializeField] GameObject bowPrefab;

    [Header("Interaction Settings")]
    [SerializeField] LayerMask interactionLayers;
    [SerializeField] float interactRange = 1.5f;

    [Header("Visuals")]
    [SerializeField] private Animator animator;
    [SerializeField] GameObject gameOver;
    private string lastDir;

    [Header("Sound")]
    [SerializeField] MusicManager musicManager;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource walkingAudioSource;
    [SerializeField] AudioClip walkingSound;
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip getHit;
    [SerializeField] AudioClip dodgeSound;
    [SerializeField] AudioClip bowPickup;
    [SerializeField] AudioClip heal;
    [SerializeField] AudioClip unlock;
    [SerializeField] AudioClip unlockGate;
    [SerializeField] AudioClip openChest;

    [SerializeField] float stepInterval = 0.25f;
    float stepTimer;
    bool firstWalk = false;

    bool isWin = false;

    void Start()
    {
        isDead = false;
        inAction = false;
        animator = GetComponent<Animator>();
        healthScript = GameObject.Find("Health").GetComponent<HealthScript>();
        playerBody = GetComponent<Rigidbody2D>();
        maxHealth = healthScript.GetMaxHealth();
        health = maxHealth;
        //audioSource = GetComponent<AudioSource>();
        stepTimer = stepInterval;
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
        if (isDead || isWin)
        {
            return;
        }
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousePos - (Vector2)transform.position;

        playerVelocity.x = Input.GetAxisRaw("Horizontal") * walkSpeed;
        playerVelocity.y = Input.GetAxisRaw("Vertical") * walkSpeed;

        if (isDodging)
        {
            return;
        }
        else
        {
            if (dodgeCooldownTimer > 0)
                dodgeCooldownTimer -= Time.deltaTime;
        }

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
        else
        {
            if (bowCooldownTimer > 0f)
                bowCooldownTimer -= Time.deltaTime;
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

        if (playerVelocity != Vector2.zero)
        {
            if (!firstWalk)
            {
                walkingAudioSource.PlayOneShot(walkingSound);
                firstWalk = true;
                stepTimer = 0f;
            }

            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval)
            {
                walkingAudioSource.PlayOneShot(walkingSound);
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f;
            firstWalk = false;
        }

        //if (!inAction && Input.GetKeyDown(KeyCode.Space))
        //{
        //    Interact();
        //}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dodge();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!isAttacking)
            {
                comboStep = 1; 
                AttackMelee(dir);
            }
            else
            {
                attackBuffered = true;
            }
        }

        // shooting bow 
        else if (Input.GetMouseButtonDown(1))
        {
            if (bowPrefab != null && bowCooldownTimer <= 0)
            {
                Vector3 pos = transform.position;
                pos.z = -0.02f;
                GameObject bow = Instantiate(bowPrefab, pos, transform.rotation, transform);
                bow.transform.localScale = new Vector3(1f / transform.lossyScale.x, 1f / transform.lossyScale.y, 1f / transform.lossyScale.z);
            }
        }
    }

    private void Interact()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, interactRange, interactionLayers);
            
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

    private void Dodge() 
    {
        if (isDodging || dodgeCooldownTimer > 0f || isAttacking || isLocked)
            return;

        Vector2 inputDir = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        if (inputDir == Vector2.zero)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            inputDir = (mousePos - (Vector2)transform.position).normalized;
        }

        //if (lastDir == "x")
        //    inputDir.y = 0;
        //else
        //    inputDir.x = 0;

        //inputDir.Normalize();

        StartCoroutine(DodgeRoll(inputDir));
    }

    private IEnumerator DodgeRoll(Vector2 direction)
    {
        audioSource.clip = dodgeSound;
        audioSource.Play();
        isDodging = true;
        isLocked = true;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            animator.Play(direction.x > 0 ? "player-run-right" : "player-run-left");
        }
        else
        {
            animator.Play(direction.y > 0 ? "player-run-up" : "player-run-down");
        }

        float timer = 0f;

        bool storedKnockback = isKnockedback;
        isKnockedback = true;

        while (timer < dodgeDuration)
        {
            playerBody.velocity = direction * dodgeSpeed;
            timer += Time.deltaTime;
            yield return null;
        }

        playerBody.velocity = Vector2.zero;
        isKnockedback = storedKnockback;

        isLocked = false;
        isDodging = false;
        dodgeCooldownTimer = dodgeCooldown;
    }

    private void AttackMelee(Vector2 dir)
    {
        isAttacking = true;
        attackBuffered = false;
        attackDirection = dir;
        audioSource.clip = attackSound;
        audioSource.Play();

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

    // deprecated fireball function
    /*private void ShootProjectile(Vector2 dir)
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
    }*/

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
        if (isDead | isDodging) { return; }
        health -= damage;

        audioSource.clip = getHit;
        audioSource.Play();

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

    public void SetBowPrefab(GameObject prefab)
    {
        bowPrefab = prefab;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetHealth(int newHealth)
    {
        health = Mathf.Clamp(newHealth, 0, maxHealth);
        healthScript.UpdateHealth(health);
    }

    public int GetKeys()
    {
        return numKeys;
    }

    public void addKey()
    {
        numKeys++;
    }


    void Death()
    {
        musicManager.StopMusic();
        playerBody.bodyType = RigidbodyType2D.Static;
        animator.Play("player-death");
        Time.timeScale = 0.5f;
        StartCoroutine(DeathAnim());
    }

    IEnumerator DeathAnim()
    {
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.5f);
        gameOver.SetActive(true);
    }

    public void Win()
    {
        musicManager.StopMusic();
        isWin = true;
        playerBody.bodyType = RigidbodyType2D.Static;
        Time.timeScale = 0.5f;
        StartCoroutine(WinAnim());
    }
    IEnumerator WinAnim()
    {
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.5f);
    }

    public void PlayerBowPickup()
    {
        audioSource.clip = bowPickup;
        audioSource.Play();
    }

    public void PlayUnlockGate()
    {
        audioSource.clip = unlockGate;
        audioSource.Play();
    }
    public void PlayUnlockOne()
    {
        audioSource.clip = unlock;
        audioSource.Play();
    }
    public void PlayHeal()
    {
        audioSource.clip = heal;
        audioSource.Play();
    }
    public void OpenChest()
    {
        audioSource.clip = openChest;
        audioSource.Play();
    }
}