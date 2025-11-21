using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class GreenChargeButton : MonoBehaviour
{
    [Header("Puzzle Settings")]
    [SerializeField] int clicksRequired = 5;
    [SerializeField] Color startColor = new Color(0.2f, 0.2f, 0.2f); // Dark Grey
    [SerializeField] Color targetColor = Color.green;

    [Header("Reward")]
    [SerializeField] GameObject chestPrefab;
    [SerializeField] float spawnX;
    [SerializeField] float spawnY;

    private int currentClicks = 0;
    private SpriteRenderer spriteRenderer;
    private bool isComplete = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateColor();
    }

    public void Interact()
    {
        // Stop interaction if already done
        if (isComplete) return;

        currentClicks++;
        Debug.Log($"Button Charge: {currentClicks}/{clicksRequired}");

        UpdateColor();

        if (currentClicks >= clicksRequired)
        {
            CompletePuzzle();
        }
    }

    private void UpdateColor()
    {
        if (spriteRenderer == null) return;

        // Calculate percentage (0.0 to 1.0)
        float progress = Mathf.Clamp01((float)currentClicks / clicksRequired);
        
        // Blend the color from Start to Target based on progress
        spriteRenderer.color = Color.Lerp(startColor, targetColor, progress);
    }

    private void CompletePuzzle()
    {
        isComplete = true;
        Debug.Log("CHARGE PUZZLE COMPLETE! Spawning Chest...");

        if (chestPrefab != null)
        {
            Vector3 spawnPos = new Vector3(spawnX, spawnY, 0f);
            Instantiate(chestPrefab, spawnPos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Puzzle Solved, but Chest Prefab is missing on the GreenChargeButton!");
        }
    }
}