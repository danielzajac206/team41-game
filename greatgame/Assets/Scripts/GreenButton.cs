using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class GreenButton : MonoBehaviour
{
    [Header("Settings")]
    public bool isWinningButton = false;
    
    [SerializeField] Color correctColor = Color.green;
    [SerializeField] Color wrongColor = new Color(0.3f, 0.5f, 0.3f); 

    private GreenButtonController controller;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        controller = FindObjectOfType<GreenButtonController>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.color = isWinningButton ? correctColor : wrongColor;
        }
    }

    public void Interact()
    {
        if (isWinningButton)
        {
             if (controller != null) 
             {
                //controller.SpawnReward();
                controller.AddCorrect();
             }
        }
    }
}