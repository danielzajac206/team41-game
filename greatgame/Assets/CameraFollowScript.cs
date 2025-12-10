using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    public GameObject player;
    private Vector2 mousePos;
    private Vector3 velocity;
    private Vector3 targetPosition = Vector3.zero;
    [SerializeField] private float damping = 0.3F;
    [SerializeField] private float cameraLimitX = 6.2F;
    [SerializeField] private float cameraLimitY = 4.55F;
    public bool camMvmntDisabled = false;
    bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        camMvmntDisabled = false;
        gameOver = false;
        Camera.main.orthographicSize = 7;
        velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (camMvmntDisabled) { 
            return; 
        }

        if (gameOver)
        {
            Vector3 targetPosition = new Vector3(player.transform.position.x - 4.37f, player.transform.position.y - 1.76f, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, damping, Mathf.Infinity, Time.unscaledDeltaTime);
        }

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.x = player.transform.position.x + ((mousePos.x - player.transform.position.x) / 2);
        targetPosition.y = player.transform.position.y + ((mousePos.y - player.transform.position.y) / 2);
        targetPosition.z = transform.position.z;

        if (targetPosition.x > (player.transform.position.x + cameraLimitX))
        {
            targetPosition.x = player.transform.position.x + cameraLimitX;
        }
        else if (targetPosition.x < (player.transform.position.x - cameraLimitX))
        {
            targetPosition.x = player.transform.position.x - cameraLimitX;
        }
        if (targetPosition.y > (player.transform.position.y + cameraLimitY))
        {
            targetPosition.y = player.transform.position.y + cameraLimitY;
        }
        else if (targetPosition.y < (player.transform.position.y - cameraLimitY))
        {
            targetPosition.y = player.transform.position.y - cameraLimitY;
        }

        // Move towards target
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, damping);
    }

    public void GameOver()
    {
        gameOver = true;
        //camMvmntDisabled = true;
        //targetPosition.x = player.transform.position.x - 4.37f;
        //targetPosition.y = player.transform.position.y - 1.76f;
        //targetPosition.z = transform.position.z;
        //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, damping, Mathf.Infinity, Time.unscaledDeltaTime);
        Debug.Log("game over");
    }
}
