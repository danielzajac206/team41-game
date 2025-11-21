using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite[] heartsSprites;

    void Start()
    {
        foreach (Image heart in hearts)
        {
            heart.sprite = heartsSprites[0];
        }
    }

    public void UpdateHealth(int health)
    {
        int maxhealth = hearts.Length;
        for (int i = maxhealth-1; i >= health; i--)
        {
            hearts[i].sprite = heartsSprites[1];
        }
    }
}
