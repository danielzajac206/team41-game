using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class ColorSwitch : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Color[] cycleColors = { Color.red, Color.green, Color.blue };
    
    public int CurrentState { get; private set; } = 0;

    private SpriteRenderer spriteRenderer;
    private ColorController controller;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        controller = FindObjectOfType<ColorController>();
        
        UpdateVisuals();
    }

    public void Interact()
    {
        CurrentState++;

        if (CurrentState >= cycleColors.Length)
        {
            CurrentState = 0;
        }

        UpdateVisuals();

        if (controller != null)
        {
            controller.CheckPuzzle();
        }
    }

    private void UpdateVisuals()
    {
        if (cycleColors.Length > 0 && spriteRenderer != null)
        {
            spriteRenderer.color = cycleColors[CurrentState];
        }
    }
}