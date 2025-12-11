using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [SerializeField] AudioClip solved;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

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
            audioSource.clip = solved;
            audioSource.Play();
            SpawnReward();
        }
    }

    private void SpawnReward()
    {
        if (chestPrefab != null)
        {
            Vector3 finalPosition = new Vector3(spawnX, spawnY, -0.01f);
            
            Instantiate(chestPrefab, finalPosition, Quaternion.identity);
            
            hasSpawned = true; 
        }
    }
}