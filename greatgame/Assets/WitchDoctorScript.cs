using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchDoctorScript : GoblinController
{
    [SerializeField] GameObject fireballPrefab;
    protected override void Move(Vector2 dir)
    {
        Vector2 moveDir = dir.normalized;
        rb.velocity = moveDir * moveSpeed;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0)
            {
                animator.Play("witchdoctor_move_right");
            }
            else
            {
                animator.Play("witchdoctor_move_left");
            }
        }
        else
        {
            if (dir.y > 0)
                animator.Play("witchdoctor_move_up");
            else
                animator.Play("witchdoctor_move_down");
        }
    }

    protected override void Attack(Vector2 dir)
    {
        isAttacking = true;
        attackTimer = attackDuration;
        rb.velocity = Vector2.zero;

        attackDirection = dir;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0)
            {
                animator.Play("witchdoctor_attack_right");
            }
            else
            {
                animator.Play("witchdoctor_attack_left");
            }
        }
        else
        {
            if (dir.y > 0)
            {
                animator.Play("witchdoctor_attack_up");
            }
            else
            {
                animator.Play("witchdoctor_attack_down");
            }
        }
    }

    protected override void CheckVision()
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
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewRadius, obstacleLayer);
                if (hit.collider == null)
                {
                    Debug.Log("player spotted");
                    Move(dir);
                }
                else
                {
                    Debug.Log("view blocked by" + hit.collider.name);
                }
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            animator.SetTrigger("Idle");
        }
    }

    public void SpawnFireball()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.z = -0.01f;

        if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
        {
            spawnPos.x += attackDirection.x > 0 ? 2 : -2;
        }
        else
        {
            spawnPos.y += attackDirection.y > 0 ? 2 : -2;
        }

        GameObject fb = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);

        if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
        {
            fb.transform.rotation = Quaternion.Euler(0, 0, attackDirection.x > 0 ? 0 : 180);
        }
        else
        {
            fb.transform.rotation = Quaternion.Euler(0, 0, attackDirection.y > 0 ? 90 : -90);
        }

        Rigidbody2D fireballRB = fb.GetComponent<Rigidbody2D>();
        if (fireballRB != null)
        {
            fireballRB.velocity = attackDirection.normalized * fb.GetComponent<FireballScript>().speed;
        }
    }

    protected override void DeathAnim(Vector2 dir)
    {
        animator.Play("witchdoctor_death");
    }
}
