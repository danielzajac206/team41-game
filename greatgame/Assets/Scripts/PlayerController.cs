using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed = 2.0f;
    [SerializeField] private Rigidbody2D playerBody;
    [SerializeField] private Vector2 playerVelocity;

    private bool inAction;

    void Start()
    {
        inAction = false;
    }

    // Update is called once per frame
    void Update()
    {
        playerVelocity.x = Input.GetAxis("Horizontal") * walkSpeed;
        playerVelocity.y = Input.GetAxis("Vertical") * walkSpeed;

        // Walking
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            playerBody.velocity = Vector2.ClampMagnitude(playerVelocity, walkSpeed);
        } 
        else
        {
            playerBody.velocity = new Vector2(0, 0);
        }

        if (!inAction && Input.GetKeyDown(KeyCode.Space))
        {
            Interact();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dodge();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void Interact()
    {
        return;
    }

    private void Dodge()
    {
        return;
    }

    private void Attack()
    {
        return;
    }
}
