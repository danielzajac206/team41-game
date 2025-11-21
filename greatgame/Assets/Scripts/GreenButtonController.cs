using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenButtonController : MonoBehaviour
{
    [Header("Reward")]
    [SerializeField] GameObject chestPrefab;
    [SerializeField] float spawnX;
    [SerializeField] float spawnY;

    private bool hasSpawned = false;

    public void SpawnReward()
    {
        // Prevent spawning multiple chests
        if (hasSpawned) return;

        if (chestPrefab != null)
        {
            Debug.Log("PUZZLE COMPLETE! Spawning Chest...");
            
            Vector3 spawnPos = new Vector3(spawnX, spawnY, 0f);
            Instantiate(chestPrefab, spawnPos, Quaternion.identity);
            
            hasSpawned = true;
        }
        else
        {
            Debug.LogWarning("Puzzle solved, but Chest Prefab is missing in GreenButtonController!");
        }
    }
}