using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    [Header("Puzzle Setup")]
    // Drag your 3 switches here
    [SerializeField] ColorSwitch[] switches; 

    [Header("Solution")]
    // 0 = Red, 1 = Green, 2 = Blue (Based on the ColorSwitch defaults)
    [SerializeField] int[] correctStates = { 0, 1, 2 };

    [Header("Reward")]
    [SerializeField] GameObject chestPrefab; 
    
    // Changed from Transform to simple X/Y coordinates
    [SerializeField] float spawnX;
    [SerializeField] float spawnY;

    private bool hasSpawned = false;

    public void CheckPuzzle()
    {
        bool isSolved = true;

        if (switches.Length != correctStates.Length)
        {
            Debug.LogWarning("ColorController: Number of switches doesn't match solution length!");
            return;
        }

        // Check every switch against the correct answer
        for (int i = 0; i < switches.Length; i++)
        {
            if (switches[i].CurrentState != correctStates[i])
            {
                isSolved = false;
                break;
            }
        }

        // Only spawn if solved AND we haven't given the reward yet
        if (isSolved && !hasSpawned)
        {
            Debug.Log("PUZZLE SOLVED! Spawning Chest...");
            SpawnReward();
        }
    }

    private void SpawnReward()
    {
        if (chestPrefab != null)
        {
            // Create a position vector from the X and Y values
            // We use 0 for Z, or you could use transform.position.z if you have depth
            Vector3 finalPosition = new Vector3(spawnX, spawnY, 0f);
            
            Instantiate(chestPrefab, finalPosition, Quaternion.identity);
            
            hasSpawned = true; 
        }
        else
        {
            Debug.LogWarning("Puzzle Solved, but you forgot to assign the Chest Prefab in the Inspector!");
        }
    }
}