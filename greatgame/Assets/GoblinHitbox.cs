using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinHitbox : MonoBehaviour
{
    public int damage = 1;
    public float lifetime = 0.4f;   

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 knockbackDir = collision.transform.position - transform.position;
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage, knockbackDir, 10f);
        }
    }
}
