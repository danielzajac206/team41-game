using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    [Header("Puzzle Setup")]
  
    [SerializeField] ColorSwitch[] switches; 

    [Header("Solution")]

    [SerializeField] int[] correctStates = { 0, 1, 2 };

    [Header("Reward")]
    [SerializeField] GameObject chestPrefab; 
    
    [SerializeField] float spawnX;
    [SerializeField] float spawnY;

    private bool hasSpawned = false;

    public void CheckPuzzle()
    {
        bool isSolved = true;

        for (int i = 0; i < switches.Length; i++)
        {
            if (switches[i].CurrentState != correctStates[i])
            {
                isSolved = false;
                break;
            }
        }

        if (isSolved && !hasSpawned)
        {
            SpawnReward();
        }
    }

    private void SpawnReward()
    {
        if (chestPrefab != null)
        {
            Vector3 finalPosition = new Vector3(spawnX, spawnY, 0f);
            
            Instantiate(chestPrefab, finalPosition, Quaternion.identity);
            
            hasSpawned = true; 
        }
    }
}