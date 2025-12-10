using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    private HashSet<Collider2D> damagedTargets = new HashSet<Collider2D>();

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !damagedTargets.Contains(collision))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage((int)damage);
                damagedTargets.Add(collision);
            }
        }
    }
}
