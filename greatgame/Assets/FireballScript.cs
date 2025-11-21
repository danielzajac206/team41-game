using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 50f;

    private Rigidbody2D rb;
    [SerializeField] Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.velocity = transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.velocity = Vector3.zero;
        animator.Play("fireball-collision");
        Debug.Log("Collision");
        Debug.Log(collision.gameObject.name);
            
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        rb.velocity = Vector3.zero;
        animator.Play("fireball-collision");
        Debug.Log("Collider");

    }

    void Explode()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                hit.GetComponent<GoblinController>()?.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}
