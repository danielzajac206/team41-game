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

    [SerializeField] AudioClip solved;
    AudioSource audioSource;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateColor();
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (isComplete) return;

        currentClicks++;

        UpdateColor();

        if (currentClicks >= clicksRequired)
        {
            CompletePuzzle();
        }
    }

    private void UpdateColor()
    {
        if (spriteRenderer == null) return;

        float progress = Mathf.Clamp01((float)currentClicks / clicksRequired);
        
        spriteRenderer.color = Color.Lerp(startColor, targetColor, progress);
    }

    private void CompletePuzzle()
    {
        isComplete = true;
        audioSource.clip = solved;
        audioSource.Play();

        if (chestPrefab != null)
        {
            Vector3 spawnPos = new Vector3(spawnX, spawnY, -0.01f);
            Instantiate(chestPrefab, spawnPos, Quaternion.identity);
        }
    }
}