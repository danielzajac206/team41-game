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
    AudioSource audioSource;
    [SerializeField] AudioClip explosion;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.velocity = transform.right * speed;
        audioSource = GetComponent<AudioSource>();
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
        audioSource.clip = explosion;
        audioSource.Play();
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Player"))
            {
                //hit.GetComponent<GoblinController>()?.TakeDamage(damage);
                Vector2 knockbackDir = hit.transform.position - transform.position;
                hit.GetComponent<PlayerController>()?.TakeDamage(damage, knockbackDir, 20f);
            }
        }

        Destroy(gameObject);
    }
}
