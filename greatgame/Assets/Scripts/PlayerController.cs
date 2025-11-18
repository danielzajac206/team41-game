using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float walkSpeed = 2.0f;
    [SerializeField] private Rigidbody2D playerBody;
    [SerializeField] private Vector2 playerVelocity;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerVelocity.x = Input.GetAxis("Horizontal") * walkSpeed;
        playerVelocity.y = Input.GetAxis("Vertical") * walkSpeed;
        //if (Input.GetKey(KeyCode.W))
        //{
        //    Vector2 movement = new Vector3(moveX, moveY, 0f);
        //    transform.position += (Vector3) movement * speed * Time.deltaTime;
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    Vector2 movement = new Vector3(moveX, moveY, 0f);
        //    transform.position += (Vector3) movement * speed * Time.deltaTime;
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    Vector2 movement = new Vector3(moveX, moveY, 0f);
        //    transform.position += (Vector3) movement * speed * Time.deltaTime;
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    Vector2 movement = new Vector3(moveX, moveY, 0f);
        //    transform.position += (Vector3) movement * speed * Time.deltaTime;
        //}

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            playerBody.velocity = Vector2.ClampMagnitude(playerVelocity, walkSpeed);
        } 
        else
        {
            playerBody.velocity = new Vector2(0, 0);
        }

        if (Input.GetKey(KeyCode.E))
        {
            Interact();
        }

    }

    private void Interact()
    {
        return;
    }
}
