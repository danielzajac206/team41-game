using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionScript : MonoBehaviour
{
    private void Start()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(EnableCollider());
    }

    IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            if (player.GetHealth() < player.GetMaxHealth())
            {
                player.SetHealth(player.GetHealth() + 2);
                player.PlayHeal();
                Destroy(gameObject);
            }
        }
    }
}
