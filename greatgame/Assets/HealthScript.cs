using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    [SerializeField] Image[] hearts;
    /* heartsSprites[0] = empty
    heartsSprites[1] = half
    heartsSprites[2] = full
    */
    [SerializeField] Sprite[] heartsSprites;
    [SerializeField] Sprite[] wholeHeartAnimation;
    [SerializeField] Sprite[] firstHalfHeartAnimation;
    [SerializeField] Sprite[] secondHalfHeartAnimation;
    private int maxHealth = 10;
    private int health;

    void Start()
    {
        maxHealth = hearts.Length * 2;
        health = maxHealth;
        foreach (Image heart in hearts)
        {
            heart.sprite = heartsSprites[2];
        }
    }
    public void SetHealth(int h)
    {
        health = Mathf.Clamp(h, 0, maxHealth);
        UpdateHealth(health);
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void AddHealth(int h)
    {
        health = Mathf.Clamp(health+h, 0, maxHealth);

    }

    public void UpdateHealth(int h)
    {
        health = Mathf.Clamp(h, 0, maxHealth);
        int healthLeft = health;
        for (int i = 0; i < hearts.Length; i++)
        {
            if (healthLeft >= 2)
            {
                hearts[i].sprite = heartsSprites[2];
                healthLeft -= 2;
            }
            else if (healthLeft == 1)
            {
                hearts[i].sprite = heartsSprites[1];
                healthLeft -= 1;
            }
            else
            {
                hearts[i].sprite = heartsSprites[0];
            }
        }
    }
}
