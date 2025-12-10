using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    [SerializeField] int damage;
    private Rigidbody2D rb;
    private Collider2D col;
    private bool stuck;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (stuck) { return; }

        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
        rb.isKinematic = true;
        col.enabled = false;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                Vector2 knockbackDir = hit.transform.position - transform.position;
                hit.GetComponent<GoblinController>()?.TakeDamage(damage, knockbackDir, 10f);
                StickToEnemy(hit.transform);
            }
        }
        StartCoroutine(DeleteArrow());
        Debug.Log("Collision");
        Debug.Log(collision.gameObject.name);

    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        rb.velocity = Vector3.zero;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                Vector2 knockbackDir = hit.transform.position - transform.position;
                hit.GetComponent<GoblinController>()?.TakeDamage(damage, knockbackDir, 5f);
            }
        }
        Debug.Log("Collider");

    }

    IEnumerator DeleteArrow()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    void StickToEnemy(Transform enemy)
    {
        stuck = true;
        transform.parent = enemy;
    }
}
