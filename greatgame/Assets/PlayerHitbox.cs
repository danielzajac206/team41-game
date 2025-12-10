using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    public int damage = 50;
    public float lifetime = 0.2f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Vector2 knockbackDir = collision.transform.position - transform.position;
            collision.gameObject.GetComponent<GoblinController>().TakeDamage(damage, knockbackDir, 10f);
        }
    }
}
