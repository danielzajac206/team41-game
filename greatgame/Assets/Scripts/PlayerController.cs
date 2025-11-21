using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    float speed = 2.0f;
    float health = 100f;
    void Start()
    {
        inAction = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.LogError("PLAYER HEALTH:" + health);

        float moveX = Input.GetAxis("Horizontal"); 
        float moveY = Input.GetAxis("Vertical");   
        if (Input.GetKey(KeyCode.W))

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

    public void playerTakeDamage(float taken) {
        health -= taken;
    }
}
