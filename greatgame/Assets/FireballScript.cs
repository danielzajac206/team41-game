using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 2;

    private Rigidbody2D rb;
    [SerializeField] Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.velocity = transform.right * speed;
        Destroy(gameObject, 5f); // Destroy after 5 seconds
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ignore collisions with other fireballs
        if (collision.gameObject.GetComponent<FireballScript>() != null)
        {
            return;
        }
        
        rb.velocity = Vector3.zero;
        animator.Play("fireball-collision");
        
        // Check if it's the player
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Ignore triggers with other fireballs
        if (collider.gameObject.GetComponent<FireballScript>() != null)
        {
            return;
        }
        
        rb.velocity = Vector3.zero;
        animator.Play("fireball-collision");
        
        // Check if it's the player
        PlayerController player = collider.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }

    void Explode()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<PlayerController>()?.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}
