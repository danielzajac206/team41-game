using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Transform player;
    public float health = 100f;

    private float fireballTimer = 3f;
    public GameObject fireballPrefab;

    void Start()
    {
        fireballTimer = 3f;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().transform;
    }
    void Update()
    {
        //code to dictate phases, kinda irrelevant rn
        if (health > 50f) {
            Phase1();
        }
        else
        {
            Phase1();
        }
    }

    void Phase1()
    {
        fireballTimer -= Time.deltaTime;

        float dist = Vector2.Distance(transform.position, player.position);

        //logic to decide what attack the boss should go for based on distance from player, irrelevant rn
        //if (dist <= tailRange) {
        //    tailSwing();
        //} 
        //else {
        //    shootFireball();
        //}

        if (fireballTimer <= 0f) shootFireball();
    }

    void shootFireball()
    {
        
        GameObject fb = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        fb.GetComponent<Fireball>().SetTarget(player.position);
        fireballTimer = 3f;
    }

}
