using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class ColorSwitch : MonoBehaviour
{
    [Header("Settings")]
    // Default to Red, Green, Blue.
    [SerializeField] Color[] cycleColors = { Color.red, Color.green, Color.blue };
    
    // The current index (0, 1, or 2)
    public int CurrentState { get; private set; } = 0;

    private SpriteRenderer spriteRenderer;
    private ColorController controller;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        controller = FindObjectOfType<ColorController>();
        
        // Initialize visuals
        UpdateVisuals();
    }

    public void Interact()
    {
        // 1. Increment state
        CurrentState++;

        // 2. Loop back to 0 if we go past the last color
        if (CurrentState >= cycleColors.Length)
        {
            CurrentState = 0;
        }

        // 3. Update Visuals
        UpdateVisuals();

        // 4. Tell the controller to check if we won
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