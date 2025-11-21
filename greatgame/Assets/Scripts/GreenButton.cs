using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class GreenButton : MonoBehaviour
{
    [Header("Settings")]
    public bool isWinningButton = false; // Check this for the ONE correct button
    
    [SerializeField] Color correctColor = Color.green;
    [SerializeField] Color wrongColor = new Color(0.3f, 0.5f, 0.3f); // Dull/Off-Green

    private GreenButtonController controller;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        controller = FindObjectOfType<GreenButtonController>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set color automatically based on if this is the winner or not
        if (spriteRenderer != null)
        {
            spriteRenderer.color = isWinningButton ? correctColor : wrongColor;
        }
    }

    public void Interact()
    {
        if (isWinningButton)
        {
             // We found the correct one!
             if (controller != null) 
             {
                 controller.SpawnReward();
             }
             else
             {
                 Debug.Log("Winner found! (But no GreenButtonController in scene to spawn chest)");
             }
        }
        else
        {
             // Optional: Feedback for clicking wrong button
             Debug.Log("Nothing happened. Wrong button.");
        }
    }
}