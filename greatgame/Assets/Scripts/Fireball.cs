using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 5f;
    public float damage = 10f;

    private Vector2 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPos,
            speed * Time.deltaTime
        );

        // Destroy when reached target
        if ((Vector2)transform.position == targetPos)
            Destroy(gameObject);
    }

    public void SetTarget(Vector2 target)
    {
        targetPos = target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController pc = collision.GetComponent<PlayerController>();
            pc.playerTakeDamage(10f);
            Destroy(gameObject);
        }
    }

}
