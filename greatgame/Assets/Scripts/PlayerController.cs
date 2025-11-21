using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] int maxHealth = 5;
    [SerializeField] HealthScript healthScript;

    [SerializeField] float walkSpeed = 2.0f;
    [SerializeField] private Rigidbody2D playerBody;
    [SerializeField] private Vector2 playerVelocity;
    [SerializeField] private Animator animator;
    [SerializeField] GameObject hitboxPrefab;
    [SerializeField] GameObject fireballPrefab;

    private bool inAction;
    //private List<KeyCode> keyStack = new List<KeyCode>();
    private string lastDir;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    private float attackDuration = 0.55f;
    private Vector2 attackDirection;

    void Start()
    {
        inAction = false;
        animator = GetComponent<Animator>();
        healthScript = GameObject.Find("Health").GetComponent<HealthScript>();
        health = maxHealth;
    }

    private void FixedUpdate()
    {
        //playerVelocity = new Vector2(x, y) * walkSpeed;
        playerBody.velocity = Vector2.ClampMagnitude(playerVelocity, walkSpeed);
    }

    // Update is called once per frame
    void Update()
    {
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

        if (isAttacking)
        {
            playerBody.velocity = new Vector2(0, 0);
            playerVelocity = new Vector2(0, 0);
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f) { 
                isAttacking = false;
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

        if (!inAction && Input.GetKeyDown(KeyCode.Space))
        {
            Interact();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dodge();
        }

        if (Input.GetMouseButtonDown(0))
        {
            AttackMelee(dir);
            Debug.Log("Attack");
        }
        if (Input.GetMouseButtonDown(1))
        {
            ShootProjectile(dir);
            Debug.Log("Fireball");
        }
    }

    private void Interact()
    {
        return;
    }

    private void Dodge()
    {
        return;
    }

    private void AttackMelee(Vector2 dir)
    {
        isAttacking = true;
        attackTimer = attackDuration;
        attackDirection = dir;
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

        if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
        {
            spawnPos.x += attackDirection.x > 0 ? 1 : -1;
        }
        else
        {
            spawnPos.y += attackDirection.y > 0 ? 1 : -1;
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

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            health = 0;
        }
        healthScript.UpdateHealth(health);
    }
}
