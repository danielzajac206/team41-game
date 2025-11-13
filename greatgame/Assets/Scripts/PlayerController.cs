using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    float speed = 2.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); 
        float moveY = Input.GetAxis("Vertical");   
        if (Input.GetKey(KeyCode.W))
        {
            Vector2 movement = new Vector3(moveX, moveY, 0f);
            transform.position += (Vector3) movement * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector2 movement = new Vector3(moveX, moveY, 0f);
            transform.position += (Vector3) movement * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector2 movement = new Vector3(moveX, moveY, 0f);
            transform.position += (Vector3) movement * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector2 movement = new Vector3(moveX, moveY, 0f);
            transform.position += (Vector3) movement * speed * Time.deltaTime;
        }

    }
}
