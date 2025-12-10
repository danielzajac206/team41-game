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
        Interact();
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

    private void Interact()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, 0.5f);

        bool foundAnything = false;

        foreach (Collider2D obj in hitObjects)
        {
            GreenChargeButton chargeButton = obj.GetComponentInParent<GreenChargeButton>();
            if (chargeButton != null)
            {
                chargeButton.Interact();
                foundAnything = true;
                return;
            }

            ColorSwitch colorSwitch = obj.GetComponentInParent<ColorSwitch>();
            if (colorSwitch != null)
            {
                colorSwitch.Interact();
                foundAnything = true;
                return;
            }

            GreenButton greenButton = obj.GetComponentInParent<GreenButton>();
            if (greenButton != null)
            {
                greenButton.Interact();
                foundAnything = true;
                return;
            }

            ChestInteraction chest = obj.GetComponentInParent<ChestInteraction>();
            if (chest != null)
            {
                chest.OpenChest();
                foundAnything = true;
                return;
            }
        }
    }
}
